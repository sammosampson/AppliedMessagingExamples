namespace Subscriber
{
    using AppliedSystems.Messaging.Messages;

    public class AddPolicyHeader : ICommand
    {
        public string TenantId { get; set; }
        public string PolicyNumber { get; set; }

        public AddPolicyHeader(string tenantId, string policyNumber)
        {
            TenantId = tenantId;
            PolicyNumber = policyNumber;
        }
    }
}