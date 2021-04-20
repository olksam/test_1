using System;
using System.Data.Common;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DbUp;
using Serilog;

namespace AccManagement.API.Data {
    public class SqlDatabase : ISqlDatabase {
        private readonly DatabaseConfig _config;
        private readonly ILogger _logger;
        private readonly DbProviderFactory _factory;

        public SqlDatabase(DatabaseConfig config, ILogger logger) {
            _config = config;
            _logger = logger;
            _factory = _config.ProviderFactory;
        }

        public async Task<DbConnection> CreateAndOpenConnection(CancellationToken cancellationToken = default) {
            var connection = _factory.CreateConnection();
            
            connection.ConnectionString = _config.ConnectionString;
            await connection.OpenAsync(cancellationToken);

            return connection;
        }

        public async Task ExecuteInTransaction(Func<DbConnection, Task> action,
            CancellationToken cancellationToken = default) {
            await using var conn = await CreateAndOpenConnection(cancellationToken);
            await using var transaction = await conn.BeginTransactionAsync(cancellationToken);
            try {
                await action.Invoke(conn);
                await transaction.CommitAsync(cancellationToken);
            } catch (Exception ex) {
                _logger.Information(ex, "Exception while executing transaction - rolling back");
                try {
                    await transaction.RollbackAsync(cancellationToken);
                } catch (Exception ex2) {
                    _logger.Error(ex2, "Error rolling back transaction");
                }

                throw;
            }
        }

        public async Task<T> ExecuteInTransaction<T>(Func<DbConnection, Task<T>> action,
            CancellationToken cancellationToken = default) {
            await using var conn = await CreateAndOpenConnection(cancellationToken);
            await using var transaction = await conn.BeginTransactionAsync(cancellationToken);
            try {
                var result = await action.Invoke(conn);
                await transaction.CommitAsync(cancellationToken);
                return result;
            } catch (Exception ex) {
                _logger.Information(ex, "Exception while executing transaction - rolling back");
                try {
                    await transaction.RollbackAsync(cancellationToken);
                } catch (Exception ex2) {
                    _logger.Error(ex2, "Error rolling back transaction");
                }

                throw;
            }
        }

        public void EnsureDbIsUpgraded() {
            _logger.Information("Updating Database.");
            
            var upgrader = DeployChanges.To
                .SQLiteDatabase(_config.ConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToAutodetectedLog()
                .Build();

            var result = upgrader.PerformUpgrade();
            if (!result.Successful) {
                _logger.Error(result.Error, "Failed to upgrade the database");
            }
        }
    }
}