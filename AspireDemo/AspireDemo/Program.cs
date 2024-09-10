var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AspireDemo_Api>("aspiredemo-api");

builder.Build().Run();
