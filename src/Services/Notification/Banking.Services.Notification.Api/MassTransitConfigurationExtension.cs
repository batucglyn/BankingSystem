using Banking.Bus.Options;
using Banking.Services.Notification.Api.Consumers;
using MassTransit;

namespace Banking.Services.Notification.Api
{
    public static class MassTransitConfigurationExtension
    {
        public static IServiceCollection AddMassTransitConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var busOptions = configuration
                .GetSection(nameof(BusOption))
                .Get<BusOption>()!;

            services.AddMassTransit(configure =>
            {
                configure.AddConsumer<AccountCreatedEventConsumer>();
                configure.AddConsumer<CustomerCreatedEventConsumer>();



                configure.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(
                        new Uri($"rabbitmq://{busOptions.Address}:{busOptions.Port}"),
                        host =>
                        {
                            host.Username(busOptions.UserName);
                            host.Password(busOptions.Password);
                        });

                    cfg.ReceiveEndpoint(
                        "notification-service.account-created.queue",
                        e =>
                        {
                            e.ConfigureConsumer<AccountCreatedEventConsumer>(ctx);
                        });
                    cfg.ReceiveEndpoint(
                            "notification-service.customer-created.queue",
                            e =>
                            {
                                e.ConfigureConsumer<CustomerCreatedEventConsumer>(ctx);
                            });
                });
            });

            return services;
        }
    }
}
