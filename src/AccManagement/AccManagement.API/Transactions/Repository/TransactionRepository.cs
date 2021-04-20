using System;
using System.Linq;
using System.Threading.Tasks;
using AccManagement.API.Common;
using AccManagement.API.Data;
using Dapper;

namespace AccManagement.API.Transactions {
    public class TransactionRepository : ITransactionRepository {
        private readonly ISqlDatabase _db;

        public TransactionRepository(ISqlDatabase db) {
            _db = db;
        }
        public async Task<OperationResult> CreateTransaction(Transaction transaction) {
            return await _db.ExecuteInTransaction(async conn => {
                var sql = @"
                    INSERT OR IGNORE INTO Accounts (AccountId, Balance, TransactionsCount) 
                    VALUES (@AccountId, 0, 0);
                      
                    INSERT OR IGNORE INTO Transactions(TransactionId, AccountId, Amount)
                    VALUES (@TransactionId, @AccountId, @Amount);";

                var rows = await conn.ExecuteAsync(sql, transaction);
                if (rows != 0) { 
                    sql = @"UPDATE Accounts 
                            SET Balance = Balance + @Amount, TransactionsCount = TransactionsCount + 1, UpdatedAt = CURRENT_TIMESTAMP 
                            WHERE AccountId = @AccountId;";
                    await conn.ExecuteAsync(sql, transaction);
                }
                return OperationResult.Ok();
            });
        }

        public async Task<TransactionDto> FindTransaction(Guid id) {
            await using var conn = await _db.CreateAndOpenConnection();

            const string sql = @"
                SELECT AccountId, Amount 
                FROM Transactions
                WHERE TransactionId = @TransactionId";

            return await conn.QueryFirstOrDefaultAsync<TransactionDto>(sql, new {TransactionId = id});
        }

        public async Task<AccountBalanceDto> FindAccountBalanceById(Guid id) {
            await using var conn = await _db.CreateAndOpenConnection();

            const string sql = @"
                SELECT Balance 
                FROM Accounts 
                WHERE AccountId = @AccountId";

            return await conn.QueryFirstOrDefaultAsync<AccountBalanceDto>(sql, new {AccountId = id});
        }

        public async Task<AccountsWithMostTransactionsDto> GetAccountsWithMostTransactions() {
            await using var conn = await _db.CreateAndOpenConnection();

            const string sql = @"
                SELECT AccountId, TransactionsCount
                FROM Accounts
                WHERE TransactionsCount = (SELECT MAX(TransactionsCount) FROM Accounts)";

            var rows = await conn.QueryAsync<(string, uint)>(sql);

            if (rows != null && rows.Any()) {
                return new AccountsWithMostTransactionsDto(rows.Select(r => Guid.Parse(r.Item1)), rows.First().Item2);
            }

            return new AccountsWithMostTransactionsDto();
        }
    }
}