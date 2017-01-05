namespace Subscriber
{
    using System;
    using AppliedSystems.Messaging.Infrastructure.Commands;

    public class AddPolicyLinesHandler : ICommandHandler<AddPolicyLines>
    {
        public void Handle(AddPolicyLines message)
        {
            Console.WriteLine($"Adding policy lines. Tenant: {message.TenantId}, PolicyNumber: {message.PolicyNumber} ");
        }
    }
}