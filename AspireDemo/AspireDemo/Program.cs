var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.AspireDemo_Api>("aspiredemo-api");

builder.AddProject<Projects.AspireDemo_Mvc>("aspiredemo-mvc")
    .WithReference(api);

builder.Build().Run();
