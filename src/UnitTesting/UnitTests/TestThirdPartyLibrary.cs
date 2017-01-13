namespace UnitTests
{
    using Subscriber;

    public class TestThirdPartyLibrary : IThirdPartyLibrary
    {
        public TestActivity LastActivityAdded { get; private set; }
        public TestPolicyHeader LastPolicyHeaderAdded { get; private set; }
        public TestPolicyLines LastPolicyLines { get; private set; }

        public void AddActivity(string tenantId, string policyNumber, string text)
        {
            LastActivityAdded = new TestActivity(tenantId, policyNumber, text);
        }

        public void AddPolicyHeader(string tenantId, string policyNumber)
        {
            LastPolicyHeaderAdded = new TestPolicyHeader(tenantId, policyNumber);
        }

        public void AddPolicyLines(string tenantId, string policyNumber)
        {
            LastPolicyLines = new TestPolicyLines(tenantId, policyNumber);
        }
    }
}