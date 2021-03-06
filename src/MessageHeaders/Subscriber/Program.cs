﻿namespace Subscriber
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
    using AppliedSystems.Messaging.Infrastructure.Receiving;
    using Claims;
    using Messages;

    class Program
    {
        static void Main(string[] args)
        {
            var eventStoreConfiguration = EventStoreSubscriptionConfiguration.FromAppConfig();

            EventStoreSubscriptionEndpoint eventStoreEndpoint = EventStoreSubscriptionEndpoint
                .ListenTo(EventStoreUrl.Parse(eventStoreConfiguration.Url))
                .WithCredentials(EventStoreUserCredentials.Parse(eventStoreConfiguration.UserCredentials.User, eventStoreConfiguration.UserCredentials.Password))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>())
                .WithInMemoryEventIndexStorage();

            MessagingFramework.Bootstrap()
                .RegisterIncomingPipelineComponent(new MessageHeadersToClaimsPipe())
                .ConfigureReceivingEndpoint(eventStoreEndpoint)
                .ConfigureMessageRouting().Incoming.ForEvents.Handle<PolicyBound>().With(new PolicyBoundHandler())
                .Initialise();

            MessageReceivingContext.MessageReceiver.StartReceiving(OnError);
            MessageReceivingContext.Events.Subscribe(PolicyEventStreamId.Parse("SimplePubSubExample"));

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
