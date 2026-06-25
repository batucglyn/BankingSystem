using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Banking.Outbox;

public sealed class OutboxProcessor<TDbContext> : BackgroundService
    where TDbContext : DbContext, IOutboxDbContext
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxProcessor<TDbContext>> _logger;

    public OutboxProcessor(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxProcessor<TDbContext>> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

    private async Task ProcessAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        var messages = await context.OutboxMessages
            .Where(x => x.ProcessedAt == null && x.RetryCount < 5)
            .OrderBy(x => x.CreatedAt)
            .Take(20)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                var type = Type.GetType(message.Type);

                if (type is null)
                {
                    message.Error = $"Type not found: {message.Type}";
                    message.RetryCount++;
                    continue;
                }

                var domainEvent = JsonSerializer.Deserialize(message.Content, type);

                if (domainEvent is null)
                {
                    message.Error = "Message content could not be deserialized.";
                    message.RetryCount++;
                    continue;
                }

                await publishEndpoint.Publish(
                    domainEvent,
                    type,
                    cancellationToken);

                message.ProcessedAt = DateTime.UtcNow;
                message.Error = null;

                _logger.LogInformation(
                    "Outbox message published. DbContext: {DbContext}, MessageId: {MessageId}, Type: {Type}",
                    typeof(TDbContext).Name,
                    message.Id,
                    message.Type);
            }
            catch (Exception ex)
            {
                message.Error = ex.Message;
                message.RetryCount++;

                _logger.LogError(
                    ex,
                    "Outbox message publish failed. DbContext: {DbContext}, MessageId: {MessageId}",
                    typeof(TDbContext).Name,
                    message.Id);
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}