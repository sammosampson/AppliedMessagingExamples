namespace SimpleEventSourcing
{
    using AppliedSystems.Domain.EventSourced;
    using AppliedSystems.Messaging.Messages;
    using SimpleEventSourcing.Messages;

    public class BankAccount : AggregateRoot<BankAccountState>
    {
        private const decimal BalanceLimit = 100;
        private const decimal OverdraftLimit = -100;

        public BankAccount() : base(new BankAccountState())
        {
        }

        public void When(DepositMoneyIntoAccount command)
        {
            if (State.Balance >= BalanceLimit)
            {
                Then(new AccountBalanceLimitReached(command.ToDeposit));
            }
            else
            {
                Then(new AccountCredited(command.ToDeposit));
            }
        }

        public void When(WithdrawMoneyFromAccount command)
        {
            if (State.Balance <= OverdraftLimit)
            {
                Then(new AccountOverdraftLimitReached(command.ToWithdraw));
            }
            else
            {
                Then(new AccountDebited(command.ToWithdraw));
            }
        }
    }
}