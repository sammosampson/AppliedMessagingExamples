
namespace SimpleEventSourcing
{
    using System;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.EventStore.Http;
    using AppliedSystems.Messaging.EventStore.Http.Configuration;
    using AppliedSystems.Messaging.EventStore.Http.SystemDot;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;
    using SimpleEventSourcing.Messages;

    class Program
    {
        static void Main(string[] args)
        {
            var eventStorageConfig = HttpEventStoreConfiguration.FromAppConfig();

            var eventStoreEndpoint = HttpEventStoreEndpoint
               .OnUrl(HttpEventStoreUrl.Parse(eventStorageConfig.Url))
               .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<DepositMoneyIntoAccount>());

            var container = new IocContainer(t => t.NameInCSharp());

            Bootstrap.Application()
                .ResolveReferencesWith(container)
                .SetupMessaging()
                    .SetupHttpEventStore()
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
