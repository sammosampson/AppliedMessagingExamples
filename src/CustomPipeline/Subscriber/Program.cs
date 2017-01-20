namespace CustomPipelineSubscriber
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SystemDot.Bootstrapping;
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.EventStore.GES;
    using AppliedSystems.Messaging.EventStore.GES.Configuration;
    using AppliedSystems.Messaging.EventStore.GES.Subscribing;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;
    using AppliedSystems.Messaging.Infrastructure.Headers;
    using AppliedSystems.Messaging.Infrastructure.Receiving;

    class Program
    {
        static void Main(string[] args)
        {
            var eventStoreConfiguration = EventStoreSubscriptionConfiguration.FromAppConfig();

            EventStoreSubscriptionEndpoint eventStoreEndpoint = EventStoreSubscriptionEndpoint
                .ListenTo(EventStoreUrl.Parse(eventStoreConfiguration.Url))
                .WithCredentials(EventStoreUserCredentials.Parse(eventStoreConfiguration.UserCredentials.User, eventStoreConfiguration.UserCredentials.Password))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.Default())
                .WithInMemoryEventIndexStorage();

            MessagingFramework.Bootstrap()
                .ConfigureReceivingEndpoint(eventStoreEndpoint)
                .RegisterIncomingPipelineComponent(new EventForwarderPipe(new CustomEventProcessor()))
                .Initialise();

            MessageReceivingContext.MessageReceiver.StartReceiving(OnError);
            MessageReceivingContext.Events.Subscribe("CustomPipelineExample");

            Console.WriteLine("I Am Subscriber");
            Console.ReadLine();

            MessageReceivingContext.MessageReceiver.StopReceiving();
        }

        private static void OnError(Exception ex, NotRequired<Message> message)
        {
            Console.WriteLine("Exception occurred whilst processing event {0}", ex.Message);
        }
    }

    public class EventForwarderPipe : IMessagePipe
    {
        private readonly CustomEventProcessor customEventProcessor;

        public EventForwarderPipe(CustomEventProcessor customEventProcessor)
        {
            this.customEventProcessor = customEventProcessor;
        }

        public NotRequired<Message> ProcessMessage(Message message)
        {
            customEventProcessor.Process(
                message.GetHeader(new MessageIdMessageHeaderKey(), s => new Guid(s), Guid.Empty),
                message.Headers.ToDictionary(h => h.Key, h => h.Value),
                message.Payload.ToString());

            // stop the message from going any further down pipe to handlers etc
            return new NotRequired<Message>();
        }
    }

    public class CustomEventProcessor
    {
        public void Process(Guid messageId, Dictionary<string, string> headers, string body)
        {
            Console.WriteLine($"MessageId (this could be stored in database at this point and used to stop repeat messages (if it already exists)): {messageId}");
            Console.WriteLine($"Headers: {headers.Describe()}");
            Console.WriteLine($"Body: {body}");
        }
    }

    public static class DisctionaryExtensions
    {
        public static string Describe<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }
    }
}
