using System.Collections.Immutable;
using Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);
builder.AddDapr();

var dashboard = builder.AddExecutable("dapr-dashboard", "dapr", ".", "dashboard")
    .WithEndpoint(port: 8080, targetPort: 8080, name: "dashboard-http", isProxied: false).ExcludeFromManifest();

var eventPlacementApi = builder.AddProject<Projects.Errow_Ticketing_EventPlacementApi>("eventplacementapi")
    .WithExternalHttpEndpoints().WithDaprSidecar(new DaprSidecarOptions()
    {
        ResourcesPaths = ImmutableHashSet.Create(Directory.GetCurrentDirectory() + "/../dapr/components"),
    });

var cartApi = builder.AddProject<Projects.Errow_Ticketing_CartApi>("cartapi")
    .WithExternalHttpEndpoints().WithDaprSidecar(new DaprSidecarOptions()
    {
        ResourcesPaths = ImmutableHashSet.Create(Directory.GetCurrentDirectory() + "/../dapr/components"),
    });


builder.AddNpmApp("angular", "../Errow.Ticketing.FrontEnd")
    .WithReference(eventPlacementApi)
    .WithReference(cartApi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
