namespace Subscriber
{
    public interface IThirdPartyLibrary
    {
        void AddActivity(string tenantId, string policyNumber, string text);

        void AddPolicyHeader(string tenantId, string policyNumber);

        void AddPolicyLines(string tenantId, string policyNumber);
    }
}