using System;
using System.Threading.Tasks;
using AccManagement.API.Common;

namespace AccManagement.API.Transactions {
    public interface ITransactionRepository {
        Task<OperationResult> CreateTransaction(Transaction transaction);
        Task<TransactionDto> FindTransaction(Guid id);
        Task<AccountBalanceDto> FindAccountBalanceById(Guid id);
        Task<AccountsWithMostTransactionsDto> GetAccountsWithMostTransactions();
    }
}