namespace Subscriber.Activities
{
    using AppliedSystems.Messaging.Infrastructure.Commands;

    public class AddActivityHandler : ICommandHandler<AddActivity>
    {
        private readonly IThirdPartyLibrary library;

        public AddActivityHandler(IThirdPartyLibrary library)
        {
            this.library = library;
        }

        public void Handle(AddActivity message)
        {
            library.AddActivity(message.TenantId, message.PolicyNumber, message.Text);
        }
    }
}