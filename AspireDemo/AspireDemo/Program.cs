var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AspireDemo_Api>("aspiredemo-api");

builder.AddProject<Projects.AspireDemo_Mvc>("aspiredemo-mvc");

builder.Build().Run();
