namespace Messages
{
    using AppliedSystems.Messaging.Messages;

    public class PolicyBound : IEvent
    {
        public PolicyBound(string tenantId, string policyNumber, string risk)
        {
            TenantId = tenantId;
            PolicyNumber = policyNumber;
            Risk = risk;
        }

        public string TenantId { get; set; }
        public string PolicyNumber { get; set; }
        public string Risk { get; set; }
    }
}