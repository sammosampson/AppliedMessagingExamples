namespace Messages
{
    using System;
    using System.Collections.ObjectModel;
    using AppliedSystems.Messaging.Messages;

    public class PolicyBound : IEvent
    {
        public string TenantId { get; set; }
        public string PolicyNumber { get; set; }
        public string Risk{ get; set; }

        public PolicyBound(string tenantId, string policyNumber, string risk)
        {
            TenantId = tenantId;
            PolicyNumber = policyNumber;
            Risk = risk;
        }
    }
}