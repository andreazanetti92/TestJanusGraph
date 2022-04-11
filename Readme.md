# JanusGraph with Apache Cassandra and Elasticsearch using Docker-Compose

**Clone** the repo or **download** the zipped folder from [github](https://github.com/andreazanetti92/janusgraph)

Open the path into the terminal or in VS Code

Run:
```
docker-compose up -d
```

## Working with Gremlin Console

In the same terminal (or if you didn't provide the -d, in another terminal) run:
```
docker exec -it jce-janusgraph bin/gremlin.sh
```


Into the gremlin console type the following 2 commands:
```
graph = TinkerGraph.open()
g = graph.traversal()
```

Now load the graph into the gremlin server by running 
```
graph.io(graphml()).readGraph('/opt/janusgraph/mydata/air-routes.graphml')
```


To check the loaded graph run ***graph*** into gremlin console. Expected output: 
```
tinkergraph[vertices:3619 edges:50148]
```


Now you're ready to traverse the graph into the gremlin-console!

## Working with JanusGraph Server and connect to it from a .NET Application
After launched the 
```
docker-compose up -d
```

Run the command 
```
docker exec -it jce-janusgraph bin/janusgraph-server.sh conf/gremlin-server-air-routes.yaml
```
   
Referencing a .NET 6 Web API's Program.cs and Gremlin.net nuget package:  

```
...
builder.Services.AddSingleton<GremlinClient>((serviceProvider) =>
{
    var gremlinServer = new GremlinServer(
        hostname: "localhost",
        port: 8182,
        enableSsl: false,
        username: null,
        password: null
        );

    var connectionPoolSettings = new ConnectionPoolSettings
    {
        MaxInProcessPerConnection = 32,
        PoolSize = 4,
        ReconnectionAttempts = 4,
        ReconnectionBaseDelay = TimeSpan.FromSeconds(1),
    };

    return new GremlinClient(
        gremlinServer: gremlinServer,
        messageSerializer: new GraphSON3MessageSerializer(new GraphSON3Reader(), new GraphSON3Writer()),
        connectionPoolSettings: connectionPoolSettings
        );
});

builder.Services.AddSingleton<GraphTraversalSource>((serviceProvider) =>
{
    GremlinClient gremlinClient = serviceProvider.GetService<GremlinClient>();
    var driverRemoteConnection = new DriverRemoteConnection(gremlinClient, "g");
    
    return g = AnonymousTraversalSource
            .Traversal()
            .WithRemote(driverRemoteConnection);
});
...
```