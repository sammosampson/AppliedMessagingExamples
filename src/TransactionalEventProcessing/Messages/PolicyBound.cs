namespace Messages
{
    using System;
    using AppliedSystems.Messaging.Messages;

    public class PolicyBound : IEvent
    {
        public PolicyBound(Guid policyNumber, string tenantId, string risk)
        {
            PolicyNumber = policyNumber;
            TenantId = tenantId;
            Risk = risk;
        }

        public Guid PolicyNumber { get; set; }
        public string TenantId { get; set; }
        public string Risk { get; set; }
    }
}