namespace SimpleEventSourcing.Messages
{
    using AppliedSystems.Messaging.Messages;

    public class AccountBalanceLimitReached : IEvent
    {
        public decimal AttemptedDeposit { get; }

        public AccountBalanceLimitReached(decimal attemptedDeposit)
        {
            AttemptedDeposit = attemptedDeposit;
        }
    }
}