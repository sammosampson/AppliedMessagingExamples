namespace Messages
{
    public class TenantPoliciesEventStreamId
    {
        private readonly string id;

        public static TenantPoliciesEventStreamId Parse(string tenantId)
        {
            return new TenantPoliciesEventStreamId($"projectionsexamplepolicies-{tenantId}");
        }

        //implicit conversion to string so we can pass this type to any string argument
        public static implicit operator string(TenantPoliciesEventStreamId from)
        {
            return from.id;
        }

        private TenantPoliciesEventStreamId(string id)
        {
            this.id = id;
        }

        public override string ToString()
        {
            return id;
        }
    }
}