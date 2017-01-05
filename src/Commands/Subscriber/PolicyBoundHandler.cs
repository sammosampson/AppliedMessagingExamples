namespace Subscriber
{
    using System;
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using Messages;

    public class PolicyBoundHandler : IEventHandler<PolicyBound>
    {           
        public void Handle(PolicyBound message)
        {
            MessageSendingContext.Bus.Send(new AddPolicyHeader(message.TenantId, message.PolicyNumber));
            MessageSendingContext.Bus.Send(new AddPolicyLines(message.TenantId, message.PolicyNumber));
            MessageSendingContext.Bus.Send(new AddActivity(message.TenantId, message.PolicyNumber, message.Risk));
        }
    }
}