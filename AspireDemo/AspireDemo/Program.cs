using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

//var connectionString = builder.AddConnectionString("ShoeMoneyDb");

var password = builder.AddParameter("password");

var redisCache = builder.AddRedis("cache");

var server = builder.AddSqlServer("SqlServer", password: password, port:3000)
    .WithDataVolume("ForecastVolume");

var database = server.AddDatabase("ForecastDb");

var rabbit = builder.AddRabbitMQ("messaging", password: password)
    .WithManagementPlugin();

var api = builder.AddProject<Projects.AspireDemo_Api>("aspiredemo-api")
    .WithReference(database)
    .WithReference(redisCache)
    .WithReference(rabbit);

builder.AddProject<Projects.AspireDemo_Mvc>("aspiredemo-mvc")    
    .WithReference(api)
    .WithReference(redisCache)
    .WithReference(rabbit);



builder.Build().Run();
