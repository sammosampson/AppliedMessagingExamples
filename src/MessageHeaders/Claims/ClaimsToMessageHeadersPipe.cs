namespace Claims
{
    using System;
    using System.Threading;
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Security;

    public class ClaimsToMessageHeadersPipe : IMessagePipe
    {
        public NotRequired<Message> ProcessMessage(Message message)
        {
            Console.WriteLine($"Adding claims to message headers for message : {message.Payload.GetType().Name}");

            return message
                .AddHeader(new EnvironmentMessageHeaderKey(), Thread.CurrentPrincipal.GetClaim<string>(new EnvironmentClaimType()).TypedValue)
                .AddHeader(new AccountRepositoryIdMessageHeaderKey(), Thread.CurrentPrincipal.GetClaim<string>(new AccountRepositoryIdClaimType()).TypedValue);
        }
    }
}