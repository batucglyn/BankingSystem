using Banking.Bus.Events;
using Banking.Services.Account.Infrastructure.Persistence.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Banking.Services.Account.Infrastructure.BackgroundServices
{
    public sealed class OutboxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OutboxProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var context = scope.ServiceProvider
                    .GetRequiredService<AccountDbContext>();

                var publishEndpoint = scope.ServiceProvider
                    .GetRequiredService<IPublishEndpoint>();

                var messages = await context.OutboxMessages
                    .Where(x => x.ProcessedAt == null && x.RetryCount < 5)
                    .OrderBy(x => x.CreatedAt)
                    .Take(20)
                    .ToListAsync(stoppingToken);
                foreach (var message in messages)
                {
                    try
                    {
                        if (message.Type == nameof(AccountCreatedEvent))
                        {
                            var accountCreatedEvent =
                                JsonSerializer.Deserialize<AccountCreatedEvent>(
                                    message.Content);

                            if (accountCreatedEvent is null)
                            {
                                message.RetryCount++;
                                message.Error = "Message content could not be deserialized.";
                                continue;
                            }

                            await publishEndpoint.Publish(
                                accountCreatedEvent,
                                stoppingToken);

                            message.ProcessedAt = DateTime.UtcNow;
                            message.Error = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        message.RetryCount++;
                        message.Error = ex.Message;
                    }
                }

                await context.SaveChangesAsync(stoppingToken);

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
