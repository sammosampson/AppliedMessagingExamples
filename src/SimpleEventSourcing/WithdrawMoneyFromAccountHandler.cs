namespace SimpleEventSourcing
{
    using AppliedSystems.Domain.EventSourced;
    using AppliedSystems.Messaging.Infrastructure.Commands;
    using SimpleEventSourcing.Messages;

    public class WithdrawMoneyFromAccountHandler : ICommandHandler<WithdrawMoneyFromAccount>
    {
        private readonly DomainRepository repository;

        public WithdrawMoneyFromAccountHandler(DomainRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(WithdrawMoneyFromAccount message)
        {
            using (repository.StartUnitOfWork())
            {
                repository
                    .Get<BankAccount>(BankAccountId.FromAccountNumber(message.AccountNumber))
                    .When(message);
            }
        }
    }
}