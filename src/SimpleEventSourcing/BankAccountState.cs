namespace SimpleEventSourcing
{
    using AppliedSystems.Domain.EventSourced;
    using SimpleEventSourcing.Messages;

    public class BankAccountState : 
        AggregateState,
        IApplyEventToState<AccountCredited>, 
        IApplyEventToState<AccountDebited>
    {
        public decimal Balance { get; private set; }

        public void Apply(AccountCredited toApply)
        {
            Balance += toApply.Amount;
        }

        public void Apply(AccountDebited toApply)
        {
            Balance -= toApply.Amount;
        }
    }
}