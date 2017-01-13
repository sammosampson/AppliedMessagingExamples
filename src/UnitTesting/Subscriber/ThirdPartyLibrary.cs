namespace Subscriber
{
    using System;

    public class ThirdPartyLibrary : IThirdPartyLibrary
    {
        public void AddActivity(string tenantId, string policyNumber, string text)
        {
            Console.WriteLine($"Adding activity for tenant: {tenantId}, policy: {policyNumber}, with text: {text}");
        }

        public void AddPolicyHeader(string tenantId, string policyNumber)
        {
            Console.WriteLine($"Adding policy header for tenant: {tenantId}, policy: {policyNumber}");
        }

        public void AddPolicyLines(string tenantId, string policyNumber)
        {
            Console.WriteLine($"Adding policy lines for tenant: {tenantId}, policy: {policyNumber}");
        }
    }
}