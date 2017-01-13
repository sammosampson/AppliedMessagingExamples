namespace UnitTests
{
    public class TestPolicyHeader
    {
        public string TenantId { get; private set; }
        public string PolicyNumber { get; private set; }

        public TestPolicyHeader(string tenantId, string policyNumber)
        {
            TenantId = tenantId;
            PolicyNumber = policyNumber;
        }
    }
}