# AppliedMessagingExamples

This repository contains contains examples on how to use the Applied messaging framework. You'll find that each example has its own readme describing it so refer there if stuck

##Setting up

To run any of the examples you will need to have access to the uk applied nuget feed for the messaging pacakges (will soon be available on the main applied feed)
You will also need to have the following services running:

- EventStore (GES)
- Applied HTTP EventStore service
- Applied HTTP EventStore subscriptions service

Setup EventStore as follows:

Download the latest version from https://geteventstore.com/downloads/
run it from admin cmd prompt with: 
<b>EventStore.ClusterNode.exe --db ./db --log ./logs --run-projections=all</b>

Setup Applied HTTP EventStore service as follows:

Download the TC build artifact containing the octopus package for the service from 

<b>http://uk-devbs01:8888/repository/downloadAll/ConnectivityShared_MessagingHttp/247641:id/artifacts.zip</b>

- Copy the service '.nupkg' file from the artifact zip to a suitable place on your machine and rename it to a '.zip', then fianlly unzip that into the same location
 - Edit the service '.config' file to set relevant url's if needed (its all setup to run local, so this is only needed for seperate machine installation)
 - Run the service exe as administrator. The service can be setup as a windows service if you wish to do so by running the exe with an "install" switch e.g "AppliedSystems.Messaging.EventStore.Http.Server.Service.exe install"

Setup Applied HTTP EventStore subscriptions service as follows:

Download and repeat the same process as above for 

<b>http://uk-devbs01:8888/repository/downloadAll/ConnectivityShared_MessagingHttpReceiving/247686:id/artifacts.zip</b>

##Where to look first
A good place to start would be the SimplePubSub example as it is as the name suggests  he simplest of configurations to follow


