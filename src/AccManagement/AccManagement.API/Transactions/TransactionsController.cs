using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace AccManagement.API.Transactions {
    [ApiController]
    public class TransactionsController : ControllerBase {
        private readonly ILogger _logger;
        private readonly ITransactionRepository _repo;
        private readonly IMemoryCache _cache;

        public TransactionsController(ILogger logger, ITransactionRepository repo, IMemoryCache cache) {
            _logger = logger;
            _repo = repo;
            _cache = cache;
        }

        [ResponseCache(Duration = 30)]
        [HttpGet("/transaction/{transactionId}")]
        public async Task<ActionResult<Transaction>> GetTransactionById(Guid transactionId) {
            var transaction = await _repo.FindTransaction(transactionId);
            if (transaction is null) {
                _logger.Information("Transaction with ID {@Id} not found", transactionId);
                return NotFound();
            }

            _logger.Information("Returning Transaction with ID {@Id}", transactionId);
            return Ok(transaction);
        }

        [HttpPost("amount")]
        public async Task<ActionResult> CreateTransaction([FromHeader(Name = "Transaction-Id"), Required]
            Guid transactionId, [FromBody] CreateTransactionRequest request) {
            var result =
                await _repo.CreateTransaction(new Transaction(transactionId, request.AccountId, request.Amount.Value));

            if (result.Success) {
                _logger.Information("Transaction with ID {@Id} ({@Request}) created", transactionId, request);
                _cache.Remove($"balance_{request.AccountId}");
                _cache.Remove("max_transaction_volume");
                return Ok();
            }

            _logger.Information("Failed to create Transaction with ID {@Id} ({@Request}). Errors: {@Errors}",
                transactionId, request, result.Errors);
            return BadRequest(result.Errors);
        }

        [HttpGet("balance/{accountId}")]
        public async Task<ActionResult> GetAccountBalance(Guid accountId) {
            var balance = await _cache.GetOrCreateAsync($"balance_{accountId}".ToString(), async entry => {
                entry.SetSlidingExpiration(TimeSpan.FromSeconds(10));
                return await _repo.FindAccountBalanceById(accountId);
            });

            if (balance is null) {
                _logger.Information("Balance with AccountId {@Id} not found", accountId);
                return NotFound();
            }

            _logger.Information("Returning Balance with AccountId {@Id}", accountId);
            return Ok(balance);
        }

        [HttpGet("max_transaction_volume")]
        public async Task<ActionResult> GetAccountsWithMostTransactions() {
            var accounts = await _cache.GetOrCreateAsync("max_transaction_volume", async entry => {
                entry.SetSlidingExpiration(TimeSpan.FromSeconds(10));
                return await _repo.GetAccountsWithMostTransactions();
            });

            _logger.Information("Returning Accounts with most Transactions");
            return Ok(accounts);
        }
    }
}