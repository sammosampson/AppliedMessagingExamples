namespace CompetingConsumerSubscriber1
{
    using System;
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using Messages;

    public class PolicyBoundHandler : IEventHandler<PolicyBound>
    {

        public void Handle(PolicyBound message)
        {
            Console.WriteLine("Handling policybound {0}", message.Describe());
        }
    }
}