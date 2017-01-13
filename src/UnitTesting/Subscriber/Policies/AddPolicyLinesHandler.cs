namespace Subscriber.Policies
{
    using AppliedSystems.Messaging.Infrastructure.Commands;

    public class AddPolicyLinesHandler : ICommandHandler<AddPolicyLines>
    {
        private readonly IThirdPartyLibrary library;

        public AddPolicyLinesHandler(IThirdPartyLibrary library)
        {
            this.library = library;
        }

        public void Handle(AddPolicyLines message)
        {
            library.AddPolicyLines(message.TenantId, message.PolicyNumber);
        }
    }
}