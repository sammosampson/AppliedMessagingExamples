namespace UnitTests
{
    using SystemDot.Bootstrapping;
    using SystemDot.Ioc;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams.InProcess;
    using AppliedSystems.Messaging.Infrastructure.InProcess;
    using Messages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Subscriber;

    [TestClass]
    public class PolicyBoundTests
    {
        private const string TenantId = "Tenant2";
        private TestThirdPartyLibrary testThirdPartyLibrary;

        [TestInitialize]
        public void Setup()
        {
            testThirdPartyLibrary = new TestThirdPartyLibrary();

            var container = new IocContainer();
            container.RegisterInstance<IThirdPartyLibrary>(() => testThirdPartyLibrary); 

            var eventStoreContext = new InProcessEventStoreContext(new InProcessMessageSendContext());
            InProcessEventStoreEndpoint storeEndpoint = InProcessEventStoreEndpoint.UsingContext(eventStoreContext);
            InProcessEventStoreSubscriptionEndpoint storeSubscriberEndpoint = InProcessEventStoreSubscriptionEndpoint.UsingContext(eventStoreContext);
            
            MessagingFramework.Bootstrap()
                .ConfigureEventStoreEndpoint(storeEndpoint)
                .ConfigureReceivingEndpoint(storeSubscriberEndpoint)
                .ConfigureMessageRouting()
                    .ForSubscriber(container)
                    .Outgoing.ForEvents.Send<PolicyBound>().ToEventStream(TenantPoliciesEventStreamId.Parse(TenantId)).ViaEndpoint(storeEndpoint)
                .Initialise();

            MessageReceivingContext.MessageReceiver.StartReceiving((_, __) => { });
            MessageReceivingContext.Events.Subscribe(TenantPoliciesEventStreamId.Parse(TenantId));
        }

        [TestMethod]
        public void ActivityShouldBeCreatedOnPolicyBound()
        {
            var policyBound = new PolicyBound(TenantId, "testPolicy1", "Risk");
            MessageSendingContext.Bus.Send(policyBound);
            Assert.IsNotNull(testThirdPartyLibrary.LastActivityAdded);
            Assert.AreSame(testThirdPartyLibrary.LastActivityAdded.TenantId, policyBound.TenantId);
            Assert.AreSame(testThirdPartyLibrary.LastActivityAdded.PolicyNumber, policyBound.PolicyNumber);
            Assert.AreSame(testThirdPartyLibrary.LastActivityAdded.Text, policyBound.Risk);

        }

        [TestMethod]
        public void PolicyHeaderShouldBeCreatedOnPolicyBound()
        {
            var policyBound = new PolicyBound(TenantId, "testPolicy1", "Risk");
            MessageSendingContext.Bus.Send(policyBound);
            Assert.IsNotNull(testThirdPartyLibrary.LastPolicyHeaderAdded);
            Assert.AreSame(testThirdPartyLibrary.LastPolicyHeaderAdded.TenantId, policyBound.TenantId);
            Assert.AreSame(testThirdPartyLibrary.LastPolicyHeaderAdded.PolicyNumber, policyBound.PolicyNumber);
        }

        [TestMethod]
        public void PolicyLinesShouldBeCreatedOnPolicyBound()
        {
            var policyBound = new PolicyBound(TenantId, "testPolicy1", "Risk");
            MessageSendingContext.Bus.Send(policyBound);
            Assert.IsNotNull(testThirdPartyLibrary.LastPolicyLines);
            Assert.AreSame(testThirdPartyLibrary.LastPolicyLines.TenantId, policyBound.TenantId);
            Assert.AreSame(testThirdPartyLibrary.LastPolicyLines.PolicyNumber, policyBound.PolicyNumber);
        }
    }
}
