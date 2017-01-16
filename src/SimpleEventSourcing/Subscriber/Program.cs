
namespace SimpleEventSourcing.Subscriber
{
    using System;
    using SystemDot.Bootstrapping;
    using AppliedSystems.Messaging.EventStore.GES;
    using AppliedSystems.Messaging.EventStore.GES.Configuration;
    using AppliedSystems.Messaging.EventStore.GES.Subscribing;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;
    using AppliedSystems.Messaging.Infrastructure.Receiving;
    using Messages;

    class Program
    {
        static void Main(string[] args)
        {
            var eventStoreConfiguration = EventStoreSubscriptionConfiguration.FromAppConfig();

            EventStoreSubscriptionEndpoint eventStoreEndpoint = EventStoreSubscriptionEndpoint
                .ListenTo(EventStoreUrl.Parse(eventStoreConfiguration.Url))
                .WithCredentials(EventStoreUserCredentials.Parse(eventStoreConfiguration.UserCredentials.User, eventStoreConfiguration.UserCredentials.Password))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<AccountCredited>())
                .WithInMemoryEventIndexStorage();

            MessagingFramework.Bootstrap()
                .ConfigureReceivingEndpoint(eventStoreEndpoint)
                .ConfigureMessageRouting()
                    .Incoming.ForEvents
                        .Handle<AccountCredited>().With<AccountCreditedHandler>()
                        .Handle<AccountDebited>().With<AccountDebitedHandler>()
                        .Handle<AccountOverdraftLimitReached>().With<AccountOverdraftLimitReachedHandler>()
                        .Handle<AccountBalanceLimitReached>().With<AccountBalanceLimitReachedHandler>()
                .Initialise();

            MessageReceivingContext.MessageReceiver.StartReceiving((ex, message) => Console.WriteLine(ex.Message));
            MessageReceivingContext.Events.Subscribe("bankaccounts");
            
            Console.WriteLine("I am the subscriber. Press Enter to exit");
            Console.ReadLine();

            MessageReceivingContext.Events.Unsubscribe("bankaccounts");
            MessageReceivingContext.MessageReceiver.StopReceiving();

        }
    }
}
