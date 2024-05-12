
var builder = DistributedApplication.CreateBuilder(args);
builder.AddDapr();
var stateStore = builder.AddDaprStateStore("statestore");

var eventPlacementApi = builder.AddProject<Projects.Errow_Ticketing_EventPlacementApi>("eventplacementapi")
    .WithExternalHttpEndpoints().WithDaprSidecar().WithReference(stateStore);

var cartApi = builder.AddProject<Projects.Errow_Ticketing_CartApi>("cartapi")
    .WithExternalHttpEndpoints().WithDaprSidecar().WithReference(stateStore);


var angular= builder.AddNpmApp("angular", "../Errow.Ticketing.FrontEnd")
    .WithReference(eventPlacementApi)
    .WithReference(cartApi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

angular.WithEnvironment("NODE_ENV", "production");


var dashboard = builder.AddExecutable("dapr-dashboard", "dapr", ".", "dashboard")
    .WithEndpoint(port: 8080, targetPort: 8080, name: "dashboard-http", isProxied: false).ExcludeFromManifest();

builder.Build().Run();