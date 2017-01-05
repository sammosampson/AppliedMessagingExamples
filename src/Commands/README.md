
#Commands

Whereas events are messages that denote that something has occured, commands are a different type of message that represents an action to be performed. For example when the event ```PolicyBound``` occurs we may want to invoice a customer as a reaction to that event ocurring and so we might send a command ```InvoiceCustomer``` which will result in the command being processed in a handler somewhere else.

There are a couple of things important about this. Firstly we then don't have to put the logic for invoicing the customer in the ```PolicyBoundHandler``` as this is handled now in an ```InvoiceCustomerHandler``` elsewhere in the system. The event handler now becomes just a kind of orchestrator to send one or more commands representing what happens when a policy is bound. Secondly, we have no idea where the command will be handled from the event handler's point of view, it could be in-process in the current service, or it could be out of process in another service, which means we can create nice decoupled architectures.

##Example
If we look in the code example at the ```Subscriber``` project and open the ```PolicyBoundHandler``` we can see that three commands are raised sperately for setting up the policy header, then the lines and finally creating an activity:

```
...
MessageSendingContext.Bus.Send(new AddPolicyHeader(message.TenantId, message.PolicyNumber));
MessageSendingContext.Bus.Send(new AddPolicyLines(message.TenantId, message.PolicyNumber));
MessageSendingContext.Bus.Send(new AddActivity(message.TenantId, message.PolicyNumber, message.Risk));
...
```

Opening up the ```Program``` class and investigating the messaging bootstrapping we can see message routing setup for the commands:

```
...
.ConfigureMessageRouting()
...
    .Internal.ForCommands
        .Handle<AddPolicyHeader>().With<AddPolicyHeaderHandler>()
        .Handle<AddPolicyLines>().With<AddPolicyLinesHandler>()
        .Handle<AddActivity>().With<AddActivityHandler>()
...
```

This is how the framework knows how to route each command to its respective handler. The ```.Internal.ForCommands``` says that we are routing the commands to handlers in the current process.

We could route commands to a different service here by using ```.Outgoing.ForCommands``` and corresponding ```.Incoming.Commands``` in the receiving service, and we have transports for both MSMQ and 0Mq to facilitate this, but this is out of scope of these examples for now.
