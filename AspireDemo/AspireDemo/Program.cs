var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("password");

var rabbit = builder.AddRabbitMQ("messaging", password: password)
    .WithManagementPlugin();

var api = builder.AddProject<Projects.AspireDemo_Api>("aspiredemo-api")
    .WithReference(rabbit);

builder.AddProject<Projects.AspireDemo_Mvc>("aspiredemo-mvc")
    .WithReference(api)
    .WithReference(rabbit);



builder.Build().Run();
