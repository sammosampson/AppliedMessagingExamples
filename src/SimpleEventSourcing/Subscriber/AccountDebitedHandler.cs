namespace SimpleEventSourcing.Subscriber
{
    using System;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using SimpleEventSourcing.Messages;

    public class AccountDebitedHandler : IEventHandler<AccountDebited>
    {
        public void Handle(AccountDebited message)
        {
            Console.WriteLine($"Account debited with: £{message.Amount}");
        }
    }
}