using System;

namespace AccManagement.API.Transactions {
    public class Transaction {
        public Guid TransactionId { get; protected set; }
        public Guid AccountId { get; protected set; }
        public int Amount { get; protected set; }

        public Transaction(Guid transactionId, Guid accountId, int amount) {
            TransactionId = transactionId;
            AccountId = accountId;
            Amount = amount;
        }

        protected Transaction() { }
    }
}