using FluentValidation;

namespace AccManagement.API.Transactions.Validation {
    public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest> {
        public CreateTransactionRequestValidator() {
            RuleFor(e => e.Amount).NotNull();
            RuleFor(e => e.AccountId).NotEmpty();
        }
    }
}