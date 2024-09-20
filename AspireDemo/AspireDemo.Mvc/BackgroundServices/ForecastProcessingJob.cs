﻿namespace RabbitConsumer;

using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class ForecastProcessingJob : BackgroundService
{
    private readonly ILogger<ForecastProcessingJob> _logger;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;
    private IConnection? _messageConnection;
    private IModel? _messageChannel;
    private EventingBasicConsumer consumer;

    public ForecastProcessingJob(ILogger<ForecastProcessingJob> logger, IConfiguration config, IServiceProvider serviceProvider, IConnection? messageConnection)
    {
        _logger = logger;
        _config = config;
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string queueName = "forecastEvents";

        _messageConnection = _serviceProvider.GetRequiredService<IConnection>();

        _messageChannel = _messageConnection.CreateModel();
        _messageChannel.QueueDeclare(queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        consumer = new EventingBasicConsumer(_messageChannel);
        consumer.Received += ProcessMessageAsync;

        _messageChannel.BasicConsume(queue: queueName,
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        consumer.Received -= ProcessMessageAsync;
        _messageChannel?.Dispose();
    }

    private void ProcessMessageAsync(object? sender, BasicDeliverEventArgs args)
    {

        string messagetext = Encoding.UTF8.GetString(args.Body.ToArray());
        _logger.LogInformation("All forecasts retrieved from the Forecast at (Consumer time: {now}). Message Text: {text}", DateTime.Now, messagetext);

        var message = args.Body;
    }
}