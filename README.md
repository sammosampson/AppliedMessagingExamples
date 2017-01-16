# AppliedMessagingExamples

This repository contains contains examples on how to use the Applied messaging framework. You'll find that each example has its own readme describing it so refer there if stuck

##Setting up

To run any of the examples you will need to have access to the uk applied nuget feed for the messaging pacakges (will soon be available on the main applied feed)
You will also need to have the EventStore (GES) running, which is setup as follows:

Download the latest version from https://geteventstore.com/downloads/
run it from admin cmd prompt with: 
<b>EventStore.ClusterNode.exe --db ./db --log ./logs --run-projections=all</b>

##Eventstore portal:
This is where eventstore can be configured and you can  browse stream content. It can be found locally at: http://127.0.0.1:2113/web/index.html#

##Examples
Going from the simplest example, and then building on that knowledge with increasing complexity we suggest going through the examples in the following order:
- [SimplePubSub](https://github.com/sammosampson/AppliedMessagingExamples/tree/master/src/SimplePubSub)
- [TransactionalEventProcessing](https://github.com/sammosampson/AppliedMessagingExamples/tree/master/src/TransactionalEventProcessing)
- [MessageHeaders](https://github.com/sammosampson/AppliedMessagingExamples/tree/master/src/MessageHeaders)
- [Commands](https://github.com/sammosampson/AppliedMessagingExamples/tree/master/src/Commands)
- ...More to follow

