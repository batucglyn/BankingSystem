using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Outbox
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOutboxProcessor<TDbContext>(
            this IServiceCollection services)
            where TDbContext : DbContext, IOutboxDbContext
        {
            services.AddHostedService<OutboxProcessor<TDbContext>>();

            return services;
        }
    }
}
