namespace SimpleEventSourcing.Subscriber
{
    using System;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using SimpleEventSourcing.Messages;

    public class AccountCreditedHandler : IEventHandler<AccountCredited>
    {
        public void Handle(AccountCredited message)
        {
            Console.WriteLine($"Account credited with: £{message.Amount}");
        }
    }
}