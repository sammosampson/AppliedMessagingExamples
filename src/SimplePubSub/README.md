
#SimplePubSub

This example contains a simple service that publishes an event to a stream to be stored in an event store and a service that subscribes to that stream and handles the event when it arrives

To run the example ensure all the event store services described in the main readme are running and all config url's are set correctly. 

Lets take a look at the code:

##Publisher

Program.cs contains all that's going on with this one so lets start there. It breaks down into two parts pretty much those being bootstrapping/setup and send ing the event.

After reading the app.config into `HttpEventStoreConfiguration` we setup our `HttpEventStoreEndpoint` which denotes the configuration for the eventstore connection, which just consists of setting the url.

The next block basically wires everything together and specifies how to rute the events we are going to be sending:

```
MessagingFramework.Bootstrap()
  .SetupMessaging()
      .ConfigureEventStoreEndpoint(eventStoreEndpoint)
      .ConfigureMessageRouting()
          .Outgoing.ForEvents
              .Send<PolicyBound>()
                  .ViaEndpoint(eventStoreEndpoint)
                  .ToEventStream(@event => PolicyEventStreamId.Parse(@event.TenantId))
```

So first we are configuring the endpoint we've setup to let the messaging framework 'know' about it. Next we are saying 'when an event of type `PolicyBound` is sent, route it to the event store endpoint to a stream specified by the `PolicyEventStreamId` that uses the TenantId property on the event.

Lower down we actually send the event:

```
MessageSendingContext.Bus.Send(new PolicyBound("SimplePubSubExample", "<Risk><DriverName>Darth Vader</DriverName></Risk>"));
```

And that's it for the publisher, pressing 'P' will send that event.


##Subscriber



