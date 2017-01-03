
namespace SimpleEventSourcing.Subscriber
{
    using System;
    using SystemDot.Bootstrapping;
    using AppliedSystems.Messaging.EventStore.Http.Subscribing;
    using AppliedSystems.Messaging.EventStore.Http.Subscribing.Configuration;
    using AppliedSystems.Messaging.EventStore.Http.Subscribing.SystemDot.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;
    using AppliedSystems.Messaging.Infrastructure.Receiving;
    using SimpleEventSourcing.Messages;

    class Program
    {
        static void Main(string[] args)
        {
            var eventSubscriptionConfig = HttpEventStoreSubscriberConfiguration.FromAppConfig();

            var eventStoreSubscriptionEndpoint = HttpEventStoreSubscriberReceivingEndpoint
                .SubscribeToEventsFrom(HttpEventStoreSubscriptionServerUrl.Parse(eventSubscriptionConfig.Url))
                .RestartConnectionWhenDownDelay(TimeSpan.FromSeconds(eventSubscriptionConfig.ConnectionDownRestartDelayInSeconds))
                .RestartConnectionWhenErrorDelay(TimeSpan.FromSeconds(eventSubscriptionConfig.ErrorRestartDelayInSeconds))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<AccountCredited>())
                .WithInMemoryEventIndexStorage();

            MessagingFramework.Bootstrap()
                .SetupHttpEventStoreSubscribing()
                .ConfigureReceivingEndpoint(eventStoreSubscriptionEndpoint)
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
