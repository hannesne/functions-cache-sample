# Distributed in-memory cache using Azure Functions Sample

## Overview

This sample shows how to maintain an in-memory cache in Azure Functions, and update it in real-time using SignalR.
The sample consists of two seperate Function apps.
The CachePublisher app is used to propagate cache updates to subscriber apps.
It acts as a SignalR server, connected to an Azure SignalR Service running in Serverless mode.
The CacheSubscriber app shows how to maintain the cache and listen for updates. It connects to the publisher app as a SignalR client.
Any messages sent to the publisher's UpdateCache endpoint, will be sent to the subscribers and returned from their RetrieveCache endpoint.
The subscriber registers the cache in it's configuration setup as a Singleton, to ensure the cache is persisted and shared between requests. It is injected into the CacheFunctions class each time the class is instantiated to serve a RetreiveCache request.

## How to use

1. Use the `/env/create-environment.ps1` script to create a SignalR service in Serverless mode:
Open the script and replace the values for $serviceName and $resourcegroup with your own values.
Running the script will then display the connection string.
1. Rename the settings file `/src/CachePublisher/local.settings.sample.json` to `local.settings.json`
1. Replace the `{signal_r_connection_string}` in the settings file with your own connection string.
1. Rename the settings file `/src/CacheSubscriber/local.settings.sample.json` to `local.settings.json`
1. Run the CachePublisher function app. This app will run on port 7073.
1. Run the CacheSubscriber function app. This app will run on port 7071.
1. Send a `POST` request to `http://localhost:7073/api/UpdateCache` with some text in the body.
1. Send a `GET` request to `http://localhost:7071/api/RetrieveCache`. It should return the values "seed1","seed2" and whatever you previously sent to the publisher app.
1. Start another instance of the subscriber app using by navigating to the subscriber directory and running `func host start --port 7072`.
1. Any future values you now send to the UpdateCache endpoint will be returned from `http://localhost:7072/api/RetrieveCache`.
It will not contain values sent prior to the starting the app.

## Excercises for the reader

Try publishing the apps to Azure. You will have to change the Uri of the publisher service in the subscribers' settings.
Try deploying the app to an App Service Plan. Check the logs in App Insights for a trace stating "message received: {cacheItem}" to see the message distribution with more than one minimum instance.

The publisher app can be updated to serve all items that need to be cached on startup.
Try adding an endpoint to the publisher that will return the seed values. This endpoint should look like the RetrieveCache endpoint in the subscriber app.
Next, update the Cache class in the subscriber to call this endpoint in the Instance method to initialise the cache.

The startup class currently waits for the cache to initialise syncronously.
Try changing this to perform the loading in a seperate thread using Task.Run, and await the loading task before returning items from the cache.

The cache might get full at some point. Try adding a mechanism to the cache to clear the oldest items when a threshold is hit.

Add a mechanism to query the cache source for cache items when a cache miss occurs.<http://localhost:7071/api/RetrieveCache>

The subscriber app currently uses the .net 5 out-of-process model to overcome dependency conflicts between the Azure Functions runtime and the latest version of the SignalR client SDK. This comes with [tradeoffs](https://docs.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide). To use an in-process model with .net core 3.1, you will need to target an older version of the SignalR client sdk.

The publisher app uses .net core 3.1 with in-process execution. Efforts to make this use .net 5 have so far been unsuccesful, as the SignalR bindings for the connection info and the message collector is injected with null values into the function.
