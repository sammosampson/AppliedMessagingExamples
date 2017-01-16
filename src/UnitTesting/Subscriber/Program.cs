namespace Subscriber
{
    using System;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Core;
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

            var container = new IocContainer();
            container.RegisterInstance<IThirdPartyLibrary, ThirdPartyLibrary>();

            var storeConfiguration = EventStoreMessageStorageConfiguration.FromAppConfig();

            EventStoreEndpoint storeEndpoint = EventStoreEndpoint
                .OnUrl(EventStoreUrl.Parse(storeConfiguration.Url))
                .WithCredentials(EventStoreUserCredentials.Parse(storeConfiguration.UserCredentials.User, storeConfiguration.UserCredentials.Password))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>());

            var storeSubscriptionConfiguration = EventStoreSubscriptionConfiguration.FromAppConfig();

            EventStoreSubscriptionEndpoint storeSubscriberEndpoint= EventStoreSubscriptionEndpoint
                .ListenTo(EventStoreUrl.Parse(storeSubscriptionConfiguration.Url))
                .WithCredentials(EventStoreUserCredentials.Parse(storeSubscriptionConfiguration.UserCredentials.User, storeSubscriptionConfiguration.UserCredentials.Password))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>())
                .WithInMemoryEventIndexStorage();

            MessagingFramework.Bootstrap()
                .ConfigureEventStoreEndpoint(storeEndpoint)
                .ConfigureReceivingEndpoint(storeSubscriberEndpoint)
                .ConfigureMessageRouting().ForSubscriber(container)
                .Initialise();

            MessageReceivingContext.MessageReceiver.StartReceiving(OnError);
            MessageReceivingContext.Events.Subscribe(TenantPoliciesEventStreamId.Parse("Tenant2"));

            Console.WriteLine("I Am Subscriber");
            Console.ReadLine();

            MessageReceivingContext.MessageReceiver.StopReceiving();
        }

        private static void OnError(Exception ex, NotRequired<Message> message)
        {
            Console.WriteLine("Exception occurred whilst processing event {0}", ex.Message);
        }
    }
}
