#Competing consumers

##Setting up
In the event store portal
- Go to projections and click 'Enable All' to turn on projections
- Add a new projection as shown here: ![new projection](https://github.com/sammosampson/AppliedMessagingExamples/blob/master/src/CompetingConsumers/By%20Tenant%20LinkTo%20Projection.PNG "new projection")

```fromCategory("competingconsumerspolicy")
  .whenAny(function(s,e) {
    linkTo('competingconsumerspolicies-' + e.body.TenantId, e);
  });
```

- Setup a new competing consumers group as shown here:
![competing consumers](https://github.com/sammosampson/AppliedMessagingExamples/blob/master/src/CompetingConsumers/competing%20consumer%20setup.PNG "competing consumers")
- Now run all the executables in the solution and start publishing

##Summary
Competing consumer subscription facilitates multiple subscribers to a single stream via a server side persistent subscription with a group. See here for more info: http://docs.geteventstore.com/introduction/3.9.0/competing-consumers/

