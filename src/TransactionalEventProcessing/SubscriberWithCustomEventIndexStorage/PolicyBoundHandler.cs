namespace SubscriberWithCustomEventIndexStorage
{
    using System;
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using Messages;
    using SubscriberWithCustomEventIndexStorage.Sdks;

    public class PolicyBoundHandler : IEventHandler<PolicyBound>
    {
        private MyThirdPartySdk sdk;

        public PolicyBoundHandler(MyThirdPartySdk sdk)
        {
            this.sdk = sdk;
        }

        public void Handle(PolicyBound message)
        {
            sdk.SaveRisk(message.PolicyNumber, message.TenantId, message.Risk);
        }
    }
}