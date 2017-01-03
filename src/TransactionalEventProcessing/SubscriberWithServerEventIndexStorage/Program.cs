namespace SubscriberWithServerEventIndexStorage
{
    using System;
    using SystemDot.Bootstrapping;
    using AppliedSystems.Core;
    using AppliedSystems.Data.Bootstrapping;
    using AppliedSystems.Messaging.EventStore.Http;
    using AppliedSystems.Messaging.EventStore.Http.Configuration;
    using AppliedSystems.Messaging.EventStore.Http.Subscribing;
    using AppliedSystems.Messaging.EventStore.Http.Subscribing.Configuration;
    using AppliedSystems.Messaging.EventStore.Http.Subscribing.SystemDot.Bootstrapping;
    using AppliedSystems.Messaging.EventStore.Http.SystemDot;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;
    using AppliedSystems.Messaging.Infrastructure.Receiving;
    using Messages;

    class Program
    {
        static void Main(string[] args)
        {
            var eventStoreConfiguration = HttpEventStoreConfiguration.FromAppConfig();
            var eventStoreSubscriptionConfiguration = HttpEventStoreSubscriberConfiguration.FromAppConfig();

            HttpEventStoreEndpoint eventStoreEndpoint = HttpEventStoreEndpoint
                .OnUrl(HttpEventStoreUrl.Parse(eventStoreConfiguration.Url))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>());

            HttpEventStoreSubscriberReceivingEndpoint eventStoreSubscriptionEndpoint = HttpEventStoreSubscriberReceivingEndpoint
                .SubscribeToEventsFrom(HttpEventStoreSubscriptionServerUrl.Parse(eventStoreSubscriptionConfiguration.Url))
                .RestartConnectionWhenDownDelay(TimeSpan.FromSeconds(eventStoreSubscriptionConfiguration.ConnectionDownRestartDelayInSeconds))
                .RestartConnectionWhenErrorDelay(TimeSpan.FromSeconds(eventStoreSubscriptionConfiguration.ErrorRestartDelayInSeconds))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>())
                .WithEventStoreEventIndexStorage();

            MessagingFramework.Bootstrap()
                .SetupDataConnectivity().WithSqlConnection()
                .SetupMessaging()
                    .SetupHttpEventStore()
                    .SetupHttpEventStoreSubscribing()
                    .ConfigureEventStoreEndpoint(eventStoreEndpoint)
                    .ConfigureReceivingEndpoint(eventStoreSubscriptionEndpoint)
                    .ConfigureMessageRouting().Incoming.ForEvents.Handle<PolicyBound>().With<PolicyBoundHandler>()
                .Initialise();

            MessageReceivingContext.MessageReceiver.StartReceiving(OnError);
            MessageReceivingContext.Events.Subscribe(PolicyEventStreamId.Parse("EventIndexStorageExample"));

            Console.WriteLine("I Am SubscriberWithServerEventIndexStorage");
            Console.ReadLine();

            MessageReceivingContext.MessageReceiver.StopReceiving();
        }

        private static void OnError(Exception ex, NotRequired<Message> message)
        {
            Console.WriteLine("Exception occurred whilst processing event {0}", ex.Message);
        }
    }
}
