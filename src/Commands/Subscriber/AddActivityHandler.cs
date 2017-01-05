namespace Subscriber
{
    using System;
    using AppliedSystems.Messaging.Infrastructure.Commands;

    public class AddActivityHandler : ICommandHandler<AddActivity>
    {
        public void Handle(AddActivity message)
        {
            Console.WriteLine($"Adding an activity. Tenant: {message.TenantId}, PolicyNumber: {message.PolicyNumber}, Text:{message.Text} ");
        }
    }
}