namespace SimpleEventSourcing.Subscriber
{
    using System;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using SimpleEventSourcing.Messages;

    public class AccountBalanceLimitReachedHandler : IEventHandler<AccountBalanceLimitReached>
    {
        public void Handle(AccountBalanceLimitReached message)
        {
            Console.WriteLine($"Account reached balance limit. Attempted deposit of: £{message.AttemptedDeposit}");
        }
    }
}