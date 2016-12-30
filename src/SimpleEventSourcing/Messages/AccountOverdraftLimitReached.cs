namespace SimpleEventSourcing.Messages
{
    using AppliedSystems.Messaging.Messages;

    public class AccountOverdraftLimitReached : IEvent
    {
        public decimal AttemptedWithdrawal { get; }

        public AccountOverdraftLimitReached(decimal attemptedWithdrawal)
        {
            AttemptedWithdrawal = attemptedWithdrawal;
        }
    }
}