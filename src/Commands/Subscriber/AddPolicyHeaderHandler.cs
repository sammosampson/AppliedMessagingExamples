namespace Subscriber
{
    using System;
    using AppliedSystems.Messaging.Infrastructure.Commands;

    public class AddPolicyHeaderHandler : ICommandHandler<AddPolicyHeader>
    {
        public void Handle(AddPolicyHeader message)
        {
            Console.WriteLine($"Adding a policy header. Tenant: {message.TenantId}, PolicyNumber: {message.PolicyNumber}");
        }
    }
}