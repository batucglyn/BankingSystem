using Banking.Outbox;
using Banking.Services.Customer.Application.Abstractions;
using Banking.Services.Customer.Application.Abstractions.Identity;
using Banking.Services.Customer.Infrastructure.Identity;
using Banking.Services.Customer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddOutboxProcessor<CustomerDbContext>();


            services.Configure<KeycloakOptions>(
            configuration.GetSection(KeycloakOptions.SectionName));

            services.AddHttpClient<IIdentityService, KeycloakIdentityService>();
            return services;
        }
    }
}
