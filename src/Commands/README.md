
#Commands

Whereas events are a message that denotes that something has occured, commands are a different type of message that represents an action to be performed. For example when the event ```PolicyBound``` occurs we may want to invoice a customer as a reaction to that event ocurring and so we might send a command ```InvoiceCustomer``` which will result in the command being processed in a handler somewhere else.

There are a couple of things important about this. Firstly we then don't have to put the logic for invoicing the customer in the ```PolicyBoundHandler``` as this is handled now in an ```InvoiceCustomerHandler``` elsewhere in the system. The event handler now becomes just a kind of orchestrator to send one or more commands representing what happens when a policy is bound. Secondly, we have no idea where the command will be handled from the event handler's point of view, it could be in-process in the current service, or it could be out of process in another service, which means we can create nice decoupled architectures.

