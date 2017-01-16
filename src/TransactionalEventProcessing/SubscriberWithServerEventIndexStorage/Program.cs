namespace SubscriberWithServerEventIndexStorage
{
    using System;
    using SystemDot.Bootstrapping;
    using AppliedSystems.Core;
    using AppliedSystems.Data.Bootstrapping;
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
            var eventStoreConfiguration = EventStoreMessageStorageConfiguration.FromAppConfig();
            var eventStoreSubscriptionConfiguration = EventStoreSubscriptionConfiguration.FromAppConfig();

            EventStoreEndpoint eventStoreEndpoint = EventStoreEndpoint
                .OnUrl(EventStoreUrl.Parse(eventStoreConfiguration.Url))
                .WithCredentials(EventStoreUserCredentials.Parse(eventStoreConfiguration.UserCredentials.User, eventStoreConfiguration.UserCredentials.Password))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>());

            EventStoreSubscriptionEndpoint eventStoreSubscriptionEndpoint = EventStoreSubscriptionEndpoint
                .ListenTo(EventStoreUrl.Parse(eventStoreSubscriptionConfiguration.Url))
                .WithCredentials(EventStoreUserCredentials.Parse(eventStoreSubscriptionConfiguration.UserCredentials.User, eventStoreSubscriptionConfiguration.UserCredentials.Password))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>())
                .WithEventStoreEventIndexStorage();

            MessagingFramework.Bootstrap()
                .SetupDataConnectivity().WithSqlConnection()
                .SetupMessaging()
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
