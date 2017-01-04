namespace Publisher
{
    using System;
    using System.Security.Claims;
    using System.Threading;
    using SystemDot.Bootstrapping;
    using AppliedSystems.Messaging.EventStore.Http;
    using AppliedSystems.Messaging.EventStore.Http.Configuration;
    using AppliedSystems.Messaging.EventStore.Http.SystemDot;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Security;
    using Claims;
    using Messages;
    using Publisher;

    class Program
    {
        static void Main()
        {
            SetupCurrentPrincipalClaims();
            WriteClaimsDescriptionToConsole();

            var eventStoreConfiguration = HttpEventStoreConfiguration.FromAppConfig();

            HttpEventStoreEndpoint eventStoreEndpoint = HttpEventStoreEndpoint.OnUrl(
                HttpEventStoreUrl.Parse(eventStoreConfiguration.Url));

            MessagingFramework.Bootstrap()
                .SetupHttpEventStore()
                .RegisterOutgoingPipelineComponent(new ClaimsToMessageHeadersPipe())
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
                        MessageSendingContext.Bus.Send(new PolicyBound("SimplePubSubExample", "<Risk><DriverName>Darth Vader</DriverName></Risk>"));
                    }
                    catch (EventEndpointException exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                }
            }
        }

        private static void WriteClaimsDescriptionToConsole()
        {
            Console.WriteLine(ClaimsDescriber.Describe());
        }

        private static void SetupCurrentPrincipalClaims()
        {
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new EnvironmentClaimType(), "Dev");
            claimsIdentity.AddClaim(new AccountRepositoryIdClaimType(), "123");
            Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);
        }
    }
}
