
#SimplePubSub

This example contains a simple service that publishes an event to a stream to be stored in an event store and a service that subscribes to that stream and handles the event when it arrives. 

A stream is a place in eventstore where a set of related events can written to. Each stream has an id, for example 'policies-AB101' which represents a set of events related to policy number AB101. As well as writing to a stream, you can also read all events from it, or subscribe to it so that you receive each event shortly after it is written. Each event in a stream has a contiguous position to allow you to read or subscribe all events from a position that you can specify - for example get me all events from 'policies-AB101' from position 2. This allows the flexibilty for different subscribers to get events historically ('Catching up') from different points in time.

To run the example ensure all the event store services described in the main readme are running and all config url's are set correctly. 

Lets take a look at the code:

##Publisher

Program.cs contains all that's going on with this one so lets start there. It breaks down into two parts pretty much those being bootstrapping/setup and send ing the event.

After reading the app.config into `HttpEventStoreConfiguration` we setup our `HttpEventStoreEndpoint` which denotes the configuration for the eventstore connection, which just consists of setting the url.

The next block basically wires everything together and specifies how to rute the events we are going to be sending:

```
MessagingFramework.Bootstrap()
  .SetupMessaging()
      .SetupHttpEventStore()
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

This time program.cs contains our bootstrapping, but `PolicyBoundHandler` is where all the magic happens, i'e our received event is processed.

First the bootstrapping:

This time we specify an `HttpEventStoreSubscriberReceivingEndpoint` which means we want to subscribe to events published to event store

```
 HttpEventStoreSubscriberReceivingEndpoint eventStoreEndpoint = HttpEventStoreSubscriberReceivingEndpoint
    .SubscribeToEventsFrom(HttpEventStoreSubscriptionServerUrl.Parse(eventStoreConfiguration.Url))
    .RestartConnectionWhenDownDelay(TimeSpan.FromSeconds(eventStoreConfiguration.ConnectionDownRestartDelayInSeconds))
    .RestartConnectionWhenErrorDelay(TimeSpan.FromSeconds(eventStoreConfiguration.ErrorRestartDelayInSeconds))
    .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>())
    .WithInMemoryEventIndexStorage();
```

Line by line we:
 - Set the eventstore subscribing service url
 - Set a time to restart the connection if the subscription service is down
 - Set a time to restart the connection if there is an error
 - Set the place to find the event types from when deserialising the event store json representation into the actual .net event. This is needed as event store is completely unaware of the actual .net type, it only stores the dimple name e.g. "PolicyBound"
 - Specify that we store the index of the last event processed by this subscriber in memory, this is how we know not to replay all events in a stream when restarting the service, although as we are using the in memory version, it is pretty useless as when we restart we will have lost the values stored and will replay the entire stream anyway. See the TransactionalEventProcessing examples for more useful going over of this.
 
```
MessagingFramework.Bootstrap()
  .SetupMessaging()
      .SetupHttpEventStoreSubscribing()
      .ConfigureReceivingEndpoint(eventStoreEndpoint)
      .ConfigureMessageRouting()
          .Incoming.ForEvents.Handle<PolicyBound>().With(new PolicyBoundHandler())
  .Initialise();
```

Again we have to tell the framework about the endpoint, and then this time we are routing incoming events of type `PolicyBound` to the `PolicyBoundHandler` where it is processed.

Now we won't actually receive anything until we start the message receiver and subscribe to the actual stream of events we want to listen to:

```
MessageReceivingContext.MessageReceiver.StartReceiving(OnError);
MessageReceivingContext.Events.Subscribe(PolicyEventStreamId.Parse("SimplePubSubExample"));
```



