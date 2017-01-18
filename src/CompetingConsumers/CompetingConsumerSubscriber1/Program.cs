namespace CompetingConsumerSubscriber1
{
    using System;
    using SystemDot.Bootstrapping;
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.EventStore.GES;
    using AppliedSystems.Messaging.EventStore.GES.Configuration;
    using AppliedSystems.Messaging.EventStore.GES.Subscribing;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;
    using Messages;

    class Program
    {
        static void Main(string[] args)
        {
            var eventStoreConfiguration = EventStoreSubscriptionConfiguration.FromAppConfig();

            EventStorePersistentSubscriptionEndpoint eventStoreEndpoint = EventStorePersistentSubscriptionEndpoint
                 .ListenTo(EventStoreUrl.Parse(eventStoreConfiguration.Url))
                 .WithCredentials(EventStoreUserCredentials.Parse(eventStoreConfiguration.UserCredentials.User, eventStoreConfiguration.UserCredentials.Password))
                 .WithBufferSizeOf(1)
                 .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>());

            MessagingFramework.Bootstrap()
                    .ConfigureReceivingEndpoint(eventStoreEndpoint)
                    .ConfigureMessageRouting().Incoming.ForEvents.Handle<PolicyBound>().With<PolicyBoundHandler>()
                .Initialise();

            MessageReceivingContext.MessageReceiver.StartReceiving(OnError);
            MessageReceivingContext.Events.Subscribe(TenantPolicyEventStreamGroup.Parse("Tenant1"));

            Console.WriteLine("I Am CompetingConsumerSubscriber1");
            Console.ReadLine();

            MessageReceivingContext.MessageReceiver.StopReceiving();
        }

        private static void OnError(Exception ex, NotRequired<Message> message)
        {
            Console.WriteLine("Exception occurred whilst processing event {0}", ex.Message);
        }
    }
}
