namespace Subscriber
{
    using AppliedSystems.Messaging.Messages;

    public class AddPolicyLines : ICommand
    {
        public string TenantId { get; set; }
        public string PolicyNumber { get; set; }

        public AddPolicyLines(string tenantId, string policyNumber)
        {
            TenantId = tenantId;
            PolicyNumber = policyNumber;
        }
    }
}