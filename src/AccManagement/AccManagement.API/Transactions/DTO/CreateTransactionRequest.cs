using System;
using System.Text.Json.Serialization;

namespace AccManagement.API.Transactions {
    public class CreateTransactionRequest {
        [JsonPropertyName("account_id")] 
        public Guid AccountId { get; set; }
        public int? Amount { get; set; }
    }
}