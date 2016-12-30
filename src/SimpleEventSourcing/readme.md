# SimpleEventSourcing

To run this example:

You must start up eventstore with the projections enabled:

EventStore.ClusterNode.exe --db ./db --log ./logs --run-projections=all

ensure all default projections (such as fromcategory etc) are enabled from the console as they are not by default:
http://127.0.0.1:2113/web/index.html#/projections (click 'enable all')

add a continuous emitting projection in eventstore called Bank Account Indexer with the body:

```
fromCategory("bankaccount")
  .whenAny(function(s,e) {
    linkTo('bankaccounts',e);
  });
``` 
This will link events from all streams in the category of bankaccount to a stream called bankaccounts so that subscribers can get all events in this category in one subscription

add another continuous emitting projection in eventstore called BankAccount Balance with the body:
```
TBD
```
TBD
