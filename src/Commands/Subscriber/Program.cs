namespace Subscriber
{
    using System;
    using SystemDot.Bootstrapping;
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.EventStore.Http.Subscribing;
    using AppliedSystems.Messaging.EventStore.Http.Subscribing.Configuration;
    using AppliedSystems.Messaging.EventStore.Http.Subscribing.SystemDot.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;
    using AppliedSystems.Messaging.Infrastructure.Receiving;
    using Messages;

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
                .WithInMemoryEventIndexStorage();

            MessagingFramework.Bootstrap()
                .SetupHttpEventStoreSubscribing()
                .ConfigureReceivingEndpoint(eventStoreEndpoint)
                .ConfigureMessageRouting()
                    .Incoming.ForEvents
                        .Handle<PolicyBound>().With(new PolicyBoundHandler())
                    .Internal.ForCommands
                        .Handle<AddPolicyHeader>().With<AddPolicyHeaderHandler>()
                        .Handle<AddPolicyLines>().With<AddPolicyLinesHandler>()
                        .Handle<AddActivity>().With<AddActivityHandler>()
                        
                .Initialise();

            MessageReceivingContext.MessageReceiver.StartReceiving(OnError);
            MessageReceivingContext.Events.Subscribe(PolicyEventStreamId.Parse("Tenant1"));

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
