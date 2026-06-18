using Banking.Services.Customer.Application.Abstractions;
using Banking.Services.Customer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContext<CustomerDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("PostgreSql"));
            });

            services.AddScoped<ICustomerDbContext>(
                provider => provider.GetRequiredService<CustomerDbContext>());

            return services;
        }
    }
}
