#TransactionalEventProcessing

Events from event store streams are allocated a position within that stream as they are appended to it.

When a service using the messaging framework subscribes to a stream using ```MessageReceivingContext.Events.Subscribe("XXX"))``` the framework subscribes to the stream and tells it to get all events from a position that it determines was the last event processed's position + 1.

The processing of an event at a subscriber and the subsequent storage of the position of that event, so that it may be used when we subscribe again (perhaps due to a service crash and restart), is what is meant by transactional event processing.

There are currently three flavours of event position storage and thus three flavours of what is loosely termed transactional event processing:

## Local SQL database event position storage
This is where we store the position of the index in a database table in the same database as where we are putting the result of processing the events into, both operations occur within the same sql transaction so they occur atomically together. This ensures that the processing of the event in this case can only happen once, unless the event position data is deleted.

If we take a look at the example code in the SubscriberWithLocalEventIndexStorage project we can see that the subscriber endpoint is set up
using ```WithSqlDatabaseEventIndexStorage``` which invokes this option:

```
 HttpEventStoreSubscriberReceivingEndpoint eventStoreEndpoint = HttpEventStoreSubscriberReceivingEndpoint
  .SubscribeToEventsFrom(HttpEventStoreSubscriptionServerUrl.Parse(eventStoreConfiguration.Url))
  .RestartConnectionWhenDownDelay(TimeSpan.FromSeconds(eventStoreConfiguration.ConnectionDownRestartDelayInSeconds))
  .RestartConnectionWhenErrorDelay(TimeSpan.FromSeconds(eventStoreConfiguration.ErrorRestartDelayInSeconds))
  .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>())
  .WithSqlDatabaseEventIndexStorage();
```

If we look in the ```PolicyBoundHandler``` class we can see that the risk from the event is being passed through a stored procedure to put the data into the database. Before this handler starts processing an event, a sql transaction is started by the framework, and once the handler has completed, the event position will be stored in the table ```EventIndexStore``` and then the transaction is committed. 

## Server side event position storage (aka event store event position storage)
This is where we after the successful processing of an event we store the position of the processed stream event in a special eventstore stream for itself. There is no sql transaction started in this case, and so if we are processing the event data into a local sql database it is possible that a crash could occur before storage of the position and therefore when we restart and subscribe again we could replay the same event. This is known as 'at least once processing', and the processing of the event must be idempotent in order for this to be of use.

If we take a look at the example code in the SubscriberWithServerEventIndexStorage project we can see that the subscriber endpoint is set up
using ```WithEventStoreEventIndexStorage``` which invokes this option:

```
 HttpEventStoreSubscriberReceivingEndpoint eventStoreEndpoint = HttpEventStoreSubscriberReceivingEndpoint
  .SubscribeToEventsFrom(HttpEventStoreSubscriptionServerUrl.Parse(eventStoreConfiguration.Url))
  .RestartConnectionWhenDownDelay(TimeSpan.FromSeconds(eventStoreConfiguration.ConnectionDownRestartDelayInSeconds))
  .RestartConnectionWhenErrorDelay(TimeSpan.FromSeconds(eventStoreConfiguration.ErrorRestartDelayInSeconds))
  .WithEventTypeFromNameResolution(EventTypeFromNameResolver.FromTypesFromAssemblyContaining<PolicyBound>())
  .WithEventStoreEventIndexStorage();
```
If we look in the ```PolicyBoundHandler``` class we can see that the risk from the event is being passed through a stored procedure to 'upsert' the data into the database. Upserting is an idempotent operation where we check for prior existence of the data before inserting it.
