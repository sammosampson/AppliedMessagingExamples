namespace CompetingConsumerSubscriber3
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
            var eventStoreSubscriptionConfiguration = EventStoreSubscriptionConfiguration.FromAppConfig();

            EventStorePersistentSubscriptionEndpoint eventStoreSubscriptionEndpoint = EventStorePersistentSubscriptionEndpoint
                .ListenTo(EventStoreUrl.Parse(eventStoreSubscriptionConfiguration.Url))
                .WithCredentials(EventStoreUserCredentials.Parse(eventStoreSubscriptionConfiguration.UserCredentials.User, eventStoreSubscriptionConfiguration.UserCredentials.Password))
                .WithBufferSizeOf(1)
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>());

            MessagingFramework.Bootstrap()
                .SetupDataConnectivity().WithSqlConnection()
                .SetupMessaging()
                    .ConfigureReceivingEndpoint(eventStoreSubscriptionEndpoint)
                    .ConfigureMessageRouting().Incoming.ForEvents.Handle<PolicyBound>().With<PolicyBoundHandler>()
                .Initialise();

            MessageReceivingContext.MessageReceiver.StartReceiving(OnError);
            MessageReceivingContext.Events.Subscribe(TenantPolicyEventStreamGroup.Parse("Tenant1"));

            Console.WriteLine("I Am CompetingConsumerSubscriber3");
            Console.ReadLine();

            MessageReceivingContext.MessageReceiver.StopReceiving();
        }

        private static void OnError(Exception ex, NotRequired<Message> message)
        {
            Console.WriteLine("Exception occurred whilst processing event {0}", ex.Message);
        }
    }
}
