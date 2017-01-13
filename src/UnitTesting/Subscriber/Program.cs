namespace Subscriber
{
    using System;
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Core;
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

            var container = new IocContainer();
            container.RegisterInstance<IThirdPartyLibrary, ThirdPartyLibrary>();

            var storeConfiguration = HttpEventStoreConfiguration.FromAppConfig();
            var storeSubscriberConfiguration = HttpEventStoreSubscriberConfiguration.FromAppConfig();

            var storeEndpoint = HttpEventStoreEndpoint.OnUrl(HttpEventStoreUrl.Parse(storeConfiguration.Url))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>());
            
            HttpEventStoreSubscriberReceivingEndpoint storeSubscriberEndpoint = HttpEventStoreSubscriberReceivingEndpoint
                .SubscribeToEventsFrom(HttpEventStoreSubscriptionServerUrl.Parse(storeSubscriberConfiguration.Url))
                .RestartConnectionWhenDownDelay(TimeSpan.FromSeconds(storeSubscriberConfiguration.ConnectionDownRestartDelayInSeconds))
                .RestartConnectionWhenErrorDelay(TimeSpan.FromSeconds(storeSubscriberConfiguration.ErrorRestartDelayInSeconds))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>())
                .WithEventStoreEventIndexStorage();

            MessagingFramework.Bootstrap()
                .SetupHttpEventStore()
                .SetupHttpEventStoreSubscribing()
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
