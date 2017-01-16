namespace ExternalContainerSubscriber
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

            var eventStoreConfiguration = EventStoreSubscriptionConfiguration.FromAppConfig();

            EventStoreSubscriptionEndpoint eventStoreEndpoint = EventStoreSubscriptionEndpoint
                .ListenTo(EventStoreUrl.Parse(eventStoreConfiguration.Url))
                .WithCredentials(EventStoreUserCredentials.Parse(eventStoreConfiguration.UserCredentials.User, eventStoreConfiguration.UserCredentials.Password))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>())
                .WithInMemoryEventIndexStorage();

            MessagingFramework.Bootstrap()
                .ConfigureReceivingEndpoint(eventStoreEndpoint)
                .ConfigureMessageRouting().Incoming.ForEvents.Handle<PolicyBound>().With(container.Resolve<PolicyBoundHandler>())
                .Initialise();

            MessageReceivingContext.MessageReceiver.StartReceiving(OnError);
            MessageReceivingContext.Events.Subscribe(PolicyEventStreamId.Parse("ContainerExample"));

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
