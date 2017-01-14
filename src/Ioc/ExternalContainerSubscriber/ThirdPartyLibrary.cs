namespace ExternalContainerSubscriber
{
    using System;

    public class ThirdPartyLibrary : IThirdPartyLibrary
    {
        public void HandlerPolicyBound()
        {
            Console.WriteLine("Handling policybound");
        }
    }
}