namespace Messages
{
    using System;

    public class PolicyEventStreamId
    {
        private readonly string id;

        public static PolicyEventStreamId Parse(Guid policyId)
        {
            return new PolicyEventStreamId($"competingconsumerspolicy-{policyId}");
        }

        //implicit conversion to string so we can pass this type to any string argument
        public static implicit operator string(PolicyEventStreamId from)
        {
            return from.id;
        }

        private PolicyEventStreamId(string id)
        {
            this.id = id;
        }

        public override string ToString()
        {
            return id;
        }
    }
}