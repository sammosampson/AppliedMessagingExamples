namespace Claims
{
    using System.Threading;
    using AppliedSystems.Security;

    public class ClaimsDescriber
    {
        public static string Describe()
        {
            return $"Claims: Environment = {Thread.CurrentPrincipal.GetClaim<string>(new EnvironmentClaimType())}, AccountRepositoryId = {Thread.CurrentPrincipal.GetClaim<string>(new AccountRepositoryIdClaimType())}";
        }
    }
}