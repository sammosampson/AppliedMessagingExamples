namespace Subscriber
{
    using SystemDot.Ioc;
    using AppliedSystems.Messaging.Infrastructure.Bootstrapping;
    using Messages;
    using Subscriber.Activities;
    using Subscriber.Policies;

    public static class MessageRoutingConfigurationExtensions
    {
        public static MessageRoutingConfiguration ForSubscriber(this MessageRoutingConfiguration config, IIocContainer container)
        {

            return config
                .Incoming.ForEvents
                .Handle<PolicyBound>().With<PolicyBoundHandler>()
                .Internal.ForCommands
                .Handle<AddPolicyHeader>().With(container.Resolve<AddPolicyHeaderHandler>())
                .Handle<AddPolicyLines>().With(container.Resolve<AddPolicyLinesHandler>())
                .Handle<AddActivity>().With(container.Resolve<AddActivityHandler>());
        }
    }
}