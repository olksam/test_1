using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace AccManagement.API.Data {
    public interface ISqlDatabase {
        Task<DbConnection> CreateAndOpenConnection(CancellationToken cancellationToken = default);
        Task ExecuteInTransaction(Func<DbConnection, Task> action, CancellationToken cancellationToken = default);
        Task<T> ExecuteInTransaction<T>(Func<DbConnection, Task<T>> action, CancellationToken cancellationToken = default);
        void EnsureDbIsUpgraded();
    }
}