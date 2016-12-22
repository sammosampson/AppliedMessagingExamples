﻿namespace ServiceA
{
    using System;
    using System.Net;
    using SystemDot.Bootstrapping;
    using AppliedSystems.Messaging.EventStore.Http;
    using AppliedSystems.Messaging.EventStore.Http.Configuration;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using Messages;

    class Program
    {
        static void Main()
        {
            var eventStoreConfiguration = HttpEventStoreConfiguration.FromAppConfig();

            HttpEventStoreEndpoint eventStoreEndpoint = HttpEventStoreEndpoint.OnUrl(
                HttpEventStoreUrl.Parse(eventStoreConfiguration.Url));

            MessagingFramework.Bootstrap()
                .SetupMessaging()
                    .ConfigureEventStoreEndpoint(eventStoreEndpoint)
                    .ConfigureMessageRouting()
                        .Outgoing.ForEvents
                            .Send<PolicyBound>()
                                .ViaEndpoint(eventStoreEndpoint)
                                .ToEventStream(@event => PolicyEventStreamId.Parse(@event.TenantId))
                .Initialise();

            Console.WriteLine("I Am Service A");

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
                        MessageSendingContext.Bus.Send(new PolicyBound("ReallyGreatTenant1", "<Risk><DriverName>Darth Vader</DriverName></Risk>"));
                    }
                    catch (EventEndpointSendingException exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                }
            }
        }
    }
}
