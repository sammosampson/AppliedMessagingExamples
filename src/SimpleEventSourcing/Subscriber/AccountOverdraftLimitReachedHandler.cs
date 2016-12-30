namespace SimpleEventSourcing.Subscriber
{
    using System;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using SimpleEventSourcing.Messages;

    public class AccountOverdraftLimitReachedHandler : IEventHandler<AccountOverdraftLimitReached>
    {
        public void Handle(AccountOverdraftLimitReached message)
        {
            Console.WriteLine($"Account reached overdraft limit. Attempted withdrawal of: £{message.AttemptedWithdrawal}");
        }
    }
}