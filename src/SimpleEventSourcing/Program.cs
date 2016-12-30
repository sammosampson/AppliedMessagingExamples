
namespace SimpleEventSourcing
{
    using System;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.EventStore.GES;
    using AppliedSystems.Messaging.EventStore.GES.Configuration;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;
    using SimpleEventSourcing.Messages;

    class Program
    {
        static void Main(string[] args)
        {
            var eventStorageConfig = EventStoreMessageStorageConfiguration.FromAppConfig();

            var eventStoreEndpoint = EventStoreEndpoint
               .OnUrl(EventStoreUrl.Parse(eventStorageConfig.Url))
               .WithCredentials(
                   EventStoreUserCredentials.Parse(
                       eventStorageConfig.UserCredentials.User,
                       eventStorageConfig.UserCredentials.Password))
               .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<DepositMoneyIntoAccount>());

            var container = new IocContainer(t => t.NameInCSharp());

            Bootstrap.Application()
                .ResolveReferencesWith(container)
                .SetupMessaging()
                    .ConfigureEventStoreEndpoint(eventStoreEndpoint)
                    .ConfigureMessageRouting()
                        .Internal.ForCommands
                        .Handle<DepositMoneyIntoAccount>().With<DepositMoneyIntoAccountHandler>()
                        .Handle<WithdrawMoneyFromAccount>().With<WithdrawMoneyFromAccountHandler>()
                .Initialise();

            while (true)
            {
                Console.WriteLine("D = Deposit £10 into account. W = Withdraw £10 from account. Esc = Exit.");

                var key = Console.ReadKey().Key;

                if (key == ConsoleKey.Escape)
                {
                    return;
                }

                if (key == ConsoleKey.D)
                {
                    MessageSendingContext.Bus.Send(new DepositMoneyIntoAccount("111111", 10));
                }

                if (key == ConsoleKey.W)
                {
                    MessageSendingContext.Bus.Send(new WithdrawMoneyFromAccount("111111", 10));
                }
            }
        }
    }
}
