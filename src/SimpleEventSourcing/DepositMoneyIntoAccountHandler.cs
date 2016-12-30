namespace SimpleEventSourcing
{
    using AppliedSystems.Domain.EventSourced;
    using AppliedSystems.Messaging.Infrastructure.Commands;
    using SimpleEventSourcing.Messages;

    public class DepositMoneyIntoAccountHandler : ICommandHandler<DepositMoneyIntoAccount>
    {
        private readonly DomainRepository repository;

        public DepositMoneyIntoAccountHandler(DomainRepository repository)
        {
            this.repository = repository;
        }

        public void Handle(DepositMoneyIntoAccount message)
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