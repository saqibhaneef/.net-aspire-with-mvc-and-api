using AspireDemo.Api.Data;
using AspireDemo.Api.Data.Seeding;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.AddRabbitMQClient("messaging");

builder.AddSqlServerDbContext<ForecastContext>("ForecastDb");
//builder.Services.AddDbContext<ForecastContext>(opt =>
//{
//    opt.UseSqlServer(builder.Configuration.GetConnectionString("ShoeMoneyDb"));
//});



builder.Services.AddTransient<Seeder>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
    using var scope = scopeFactory.CreateScope();

    var ctx = scope.ServiceProvider.GetRequiredService<ForecastContext>();
    ctx.Database.EnsureCreated();

    // Enqueue the seeding
    var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
    seeder.Seed();
}

app.MapDefaultEndpoints();

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
