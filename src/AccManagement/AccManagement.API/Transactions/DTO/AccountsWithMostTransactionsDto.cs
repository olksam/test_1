using System;
using System.Collections.Generic;

namespace AccManagement.API.Transactions {
    public class AccountsWithMostTransactionsDto {
        public IEnumerable<Guid> Accounts { get; protected set; }
        public uint MaxVolume { get; protected set; }

        public AccountsWithMostTransactionsDto() : this(new Guid[0], 0) { }

        public AccountsWithMostTransactionsDto(IEnumerable<Guid> accounts, uint maxVolume) {
            Accounts = accounts;
            MaxVolume = maxVolume;
        }
    }
}