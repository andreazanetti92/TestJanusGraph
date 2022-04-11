using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Remote;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure;
using Gremlin.Net.Structure.IO.GraphSON;
using TestJanusGraph.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
    
    var g = AnonymousTraversalSource
            .Traversal()
            .WithRemote(driverRemoteConnection);

    //g.Io<Graph>("/opt/janusgraph/mydata/air-routes.graphml")
    //    .With(IO.reader, IO.graphml).Read();
    return g;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Try/Catch to load a graph
//try
//{
//    var g = app.Services.GetService<GraphTraversalSource>();
//    if (g != null) Seed.LoadGraph(g);
//    else throw new Exception("Cannot get service GraphTraversalSource to load data");
//}
//catch (Exception ex)
//{
//    throw new Exception("Fail to load the graph", ex);
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
