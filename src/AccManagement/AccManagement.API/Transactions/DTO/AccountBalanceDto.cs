namespace AccManagement.API.Transactions {
    public class AccountBalanceDto {
        public int Balance { get; protected set; }

        public AccountBalanceDto(int balance) {
            Balance = balance;
        }

        protected AccountBalanceDto() { }
    }
}