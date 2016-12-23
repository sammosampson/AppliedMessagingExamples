namespace SubscriberWithCustomEventIndexStorage
{
    using System;
    using SystemDot.Bootstrapping;
    using AppliedSystems.Core;
    using AppliedSystems.Data.Bootstrapping;
    using AppliedSystems.Messaging.EventStore.Http.Subscribing;
    using AppliedSystems.Messaging.EventStore.Http.Subscribing.Configuration;
    using AppliedSystems.Messaging.EventStore.Http.Subscribing.SystemDot.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;
    using Messages;
    using SubscriberWithCustomEventIndexStorage.Sdks;

    class Program
    {
        static void Main(string[] args)
        {
            var eventStoreConfiguration = HttpEventStoreSubscriberConfiguration.FromAppConfig();

            HttpEventStoreSubscriberReceivingEndpoint eventStoreEndpoint = HttpEventStoreSubscriberReceivingEndpoint
                .SubscribeToEventsFrom(HttpEventStoreSubscriptionServerUrl.Parse(eventStoreConfiguration.Url))
                .RestartConnectionWhenDownDelay(TimeSpan.FromSeconds(eventStoreConfiguration.ConnectionDownRestartDelayInSeconds))
                .RestartConnectionWhenErrorDelay(TimeSpan.FromSeconds(eventStoreConfiguration.ErrorRestartDelayInSeconds))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>())
                .WithMyThirdPartySdkEventIndexStorage();

            MessagingFramework.Bootstrap()
                .SetupDataConnectivity().WithSqlConnection()
                .SetupMessaging()
                    .SetupHttpMessageReceiving()
                    .ConfigureReceivingEndpoint(eventStoreEndpoint)
                    .ConfigureMessageRouting()
                        .Incoming.ForEvents.Handle<PolicyBound>().With<PolicyBoundHandler>()
                .Initialise();

            MessageReceivingContext.MessageReceiver.StartReceiving(OnError);
            MessageReceivingContext.Events.Subscribe(PolicyEventStreamId.Parse("EventIndexStorageExample"));

            Console.WriteLine("I Am SubscriberWithCustomEventIndexStorage");
            Console.ReadLine();

            MessageReceivingContext.MessageReceiver.StopReceiving();
        }

        private static void OnError(Exception ex, NotRequired<Message> message)
        {
            Console.WriteLine("Exception occurred whilst processing event {0}", ex.Message);
        }
    }
}
