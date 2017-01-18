namespace Messages
{
    public class TenantPolicyEventStreamGroup
    {
        private readonly string id;

        public static TenantPolicyEventStreamGroup Parse(string tenant)
        {
            return new TenantPolicyEventStreamGroup($"competingconsumerspolicies-{tenant}/testgroup");
        }

        //implicit conversion to string so we can pass this type to any string argument
        public static implicit operator string(TenantPolicyEventStreamGroup from)
        {
            return from.id;
        }

        private TenantPolicyEventStreamGroup(string id)
        {
            this.id = id;
        }

        public override string ToString()
        {
            return id;
        }
    }
}