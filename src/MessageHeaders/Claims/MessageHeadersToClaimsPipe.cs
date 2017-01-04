namespace Claims
{
    using System;
    using System.Security.Claims;
    using System.Threading;
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Security;

    public class MessageHeadersToClaimsPipe : IMessagePipe
    {
        public NotRequired<Message> ProcessMessage(Message message)
        {
            Console.WriteLine($"Getting claims from message headers for message : {message.Payload.GetType().Name}");

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new EnvironmentClaimType(), message.GetHeader(new EnvironmentMessageHeaderKey()));
            claimsIdentity.AddClaim(new AccountRepositoryIdClaimType(), message.GetHeader(new AccountRepositoryIdMessageHeaderKey()));
            Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);
            return message;
        }
    }
}