namespace SimpleEventSourcing.Messages
{
    using AppliedSystems.Messaging.Messages;

    public class AccountCredited : IEvent
    {
        public decimal Amount { get; }

        public AccountCredited(decimal amount)
        {
            Amount = amount;
        }
    }
}