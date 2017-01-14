namespace Messages
{
    using AppliedSystems.Messaging.Messages;

    public class PolicyBound : IEvent
    {
        public PolicyBound(string tenantId, string risk)
        {
            TenantId = tenantId;
            Risk = risk;
        }

        public string TenantId { get; set; }
        public string Risk { get; set; }
    }
}