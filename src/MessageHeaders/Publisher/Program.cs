namespace Publisher
{
    using System;
    using System.Security.Claims;
    using System.Threading;
    using SystemDot.Bootstrapping;
    using AppliedSystems.Messaging.EventStore.GES;
    using AppliedSystems.Messaging.EventStore.GES.Configuration;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;
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

            var eventStoreConfiguration = EventStoreMessageStorageConfiguration.FromAppConfig();

            EventStoreEndpoint eventStoreEndpoint = EventStoreEndpoint
                .OnUrl(EventStoreUrl.Parse(eventStoreConfiguration.Url))
                .WithCredentials(EventStoreUserCredentials.Parse(eventStoreConfiguration.UserCredentials.User, eventStoreConfiguration.UserCredentials.Password))
                .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>());

            MessagingFramework.Bootstrap()
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
                    MessageSendingContext.Bus.Send(new PolicyBound("SimplePubSubExample", "<Risk><DriverName>Darth Vader</DriverName></Risk>"));
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
