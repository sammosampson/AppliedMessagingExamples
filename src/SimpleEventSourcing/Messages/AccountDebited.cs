namespace SimpleEventSourcing.Messages
{
    using AppliedSystems.Messaging.Messages;

    public class AccountDebited : IEvent
    {
        public decimal Amount { get; }

        public AccountDebited(decimal amount)
        {
            Amount = amount;
        }
    }
}