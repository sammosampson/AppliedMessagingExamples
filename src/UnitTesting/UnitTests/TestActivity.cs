namespace UnitTests
{
    public class TestActivity
    {
        public string TenantId { get; private set; }
        public string PolicyNumber { get; private set; }
        public string Text { get; private set; }

        public TestActivity(string tenantId, string policyNumber, string text)
        {
            TenantId = tenantId;
            PolicyNumber = policyNumber;
            Text = text;
        }
    }
}