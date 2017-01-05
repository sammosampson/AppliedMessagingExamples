namespace Subscriber
{
    using AppliedSystems.Messaging.Messages;

    public class AddActivity : ICommand
    {
        public string TenantId { get; set; }
        public string PolicyNumber { get; set; }
        public string Text { get; set; }

        public AddActivity(string tenantId, string policyNumber, string text)
        {
            TenantId = tenantId;
            PolicyNumber = policyNumber;
            Text = text;
        }
    }
}