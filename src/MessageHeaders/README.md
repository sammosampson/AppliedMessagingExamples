
#MessageHeaders

Event store has the ability to store metadata along with the event data in a stream. This is useful for information that you want to always pass around with every message but don't necessarily want it in the body of the actual event, cluttering its intent. For example you might want to pass around the current environment, "dev" or "production".

The messaging framework provides access to this meta data storage and retrieval via headers on each message envelope, which is how messages pass through the framework. Message headers can be read and set using a Message Processing Pipe, which is something you slot into the overall message pipeline.

##Example
The code example here takes some current principal claims that are set up on initialisation of the publisher, and serialises them to message headers as each event is sent. The reverse of this happens when the event is received at the subscriber, converting the headers back into claims.

If we take a look at the ```Program``` class in the ```Publisher``` project in the example code we can see that we initially setup some claims on the current security principal for the current environment and account repository:
(NOTE: we are using the AppliedSystems.Security nuget package for the Claims helper extensions you can see here, but there is no reason why this has to be used over the BCL, it just makes for more condensed code for example purposes)

```
private static void SetupCurrentPrincipalClaims()
{
    var claimsIdentity = new ClaimsIdentity();
    claimsIdentity.AddClaim(new EnvironmentClaimType(), "Dev");
    claimsIdentity.AddClaim(new AccountRepositoryIdClaimType(), "123");
    Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);
}
```

Now if we look back at the message framework bootstrapping there is a line that registers a ```ClaimsToMessageHeadersPipe```  in the outgoing message pipeline:

```
...
.RegisterOutgoingPipelineComponent(new ClaimsToMessageHeadersPipe())
...
```

The pipe is used to process a message as it is being sent, and this one basically converts the claims into message headers on the current message:

```
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
```

As can be seen, the event itself is contained in the ```Payload``` property of the ```Message```and headers can be added to the ```Message``` using the ```AddHeader``` method, which creates a new version of the ```Message``` containing the new header. 

Admittedly the ```Message``` class is rather poorly named and would be better represented by something like ```MessageEnvelope``` to avoid confusion!

In the ```Subscriber``` project, the same exists but in reverse in order to deserialise message headers back into claims. First we register the ```MessageHeadersToClaimsPipe``` pipe this time on the incoming pipeline using:

```
...
.RegisterOutgoingPipelineComponent(new ClaimsToMessageHeadersPipe())
...
```

And here is the code for the pipe:

```
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
```

You can see how the headers are stored as metadata in eventstore by looking at the events in the stream fro this example in the eventstore portal, locally at: http://127.0.0.1:2113/web/index.html#/streams
