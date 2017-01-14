namespace NoContainerSubscriber
{
    using AppliedSystems.Messaging.Infrastructure.Events;
    using Messages;

    public class PolicyBoundHandler : IEventHandler<PolicyBound>
    {
        private readonly IThirdPartyLibrary thirdPartyLibrary;

        public PolicyBoundHandler(IThirdPartyLibrary thirdPartyLibrary)
        {
            this.thirdPartyLibrary = thirdPartyLibrary;
        }

        public void Handle(PolicyBound message)
        {
            thirdPartyLibrary.HandlerPolicyBound();
        }
    }
}