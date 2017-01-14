namespace Publisher
{
    using System;
    using SystemDot.Bootstrapping;
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.EventStore.Http;
    using AppliedSystems.Messaging.EventStore.Http.Configuration;
    using AppliedSystems.Messaging.EventStore.Http.SystemDot;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Headers;
    using Messages;

    class Program
    {
        static void Main()
        {
            var eventStoreConfiguration = HttpEventStoreConfiguration.FromAppConfig();

            HttpEventStoreEndpoint eventStoreEndpoint = HttpEventStoreEndpoint.OnUrl(
                HttpEventStoreUrl.Parse(eventStoreConfiguration.Url));

            MessagingFramework.Bootstrap()
                .SetupHttpEventStore()
                .ConfigureEventStoreEndpoint(eventStoreEndpoint)
                .ConfigureMessageRouting()
                    .Outgoing.ForEvents
                        .Send<PolicyBound>()
                            .ViaEndpoint(eventStoreEndpoint)
                            .ToEventStream(@event => PolicyEventStreamId.Parse(@event.TenantId))
                .Initialise();

            Console.WriteLine("I Am Publisher");

            while (true)
            {
                Console.WriteLine("P = Raise PolicyBound event. Esc = Exit.");
                var key = Console.ReadKey().Key;

                if (key == ConsoleKey.Escape)
                {
                    return;
                }

                if (key == ConsoleKey.P)
                {
                    try
                    {
                        MessageSendingContext.Bus.Send(new PolicyBound("ContainerExample", "<Risk><DriverName>Darth Vader</DriverName></Risk>"));
                    }
                    catch (EventEndpointException exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                }
            }
        }
    }
}
