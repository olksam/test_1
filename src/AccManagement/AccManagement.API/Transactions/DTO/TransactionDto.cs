using System;
using System.Text.Json.Serialization;

namespace AccManagement.API.Transactions {
    public class TransactionDto {
        [JsonPropertyName("account_id")]
        public Guid AccountId { get; protected set; }
        public int Amount { get; protected set; }

        public TransactionDto(Guid accountId, int amount) {
            AccountId = accountId;
            Amount = amount;
        }

        protected TransactionDto() { }
    }
}