namespace UnitTests
{
    public class TestPolicyLines
    {
        public string TenantId { get; private set; }
        public string PolicyNumber { get; private set; }

        public TestPolicyLines(string tenantId, string policyNumber)
        {
            TenantId = tenantId;
            PolicyNumber = policyNumber;
        }
    }
}