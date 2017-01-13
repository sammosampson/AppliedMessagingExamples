namespace Subscriber.Policies
{
    using AppliedSystems.Messaging.Infrastructure.Commands;

    public class AddPolicyHeaderHandler : ICommandHandler<AddPolicyHeader>
    {
        private readonly IThirdPartyLibrary library;

        public AddPolicyHeaderHandler(IThirdPartyLibrary library)
        {
            this.library = library;
        }

        public void Handle(AddPolicyHeader message)
        {
            library.AddPolicyHeader(message.TenantId, message.PolicyNumber);
        }
    }
}