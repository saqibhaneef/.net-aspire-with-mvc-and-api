using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var connectionString = builder.AddConnectionString("ShoeMoneyDb");

var password = builder.AddParameter("password");

var rabbit = builder.AddRabbitMQ("messaging", password: password)
    .WithManagementPlugin();

var api = builder.AddProject<Projects.AspireDemo_Api>("aspiredemo-api")
    //.WithEnvironment("ConnectionStrings:ShoeMoneyDb", builder.Configuration.GetConnectionString("ShoeMoneyDb"))
    .WithReference(connectionString)
    .WithReference(rabbit);

builder.AddProject<Projects.AspireDemo_Mvc>("aspiredemo-mvc")    
    .WithReference(api)
    .WithReference(rabbit);



builder.Build().Run();
