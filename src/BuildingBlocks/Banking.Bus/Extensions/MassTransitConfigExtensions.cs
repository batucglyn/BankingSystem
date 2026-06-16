using Banking.Bus.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Text;
namespace Banking.Bus.Extensions
{
    public static class MassTransitConfigExtensions
    {
        public static IServiceCollection AddCommonMassTransit(this IServiceCollection services,
      IConfiguration configuration)
        {
            var busOptions = configuration.GetSection(nameof(BusOption)).Get<BusOption>()!;


            services.AddMassTransit(configure =>
            {
                configure.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri($"rabbitmq://{busOptions.Address}:{busOptions.Port}"), host =>
                    {
                        host.Username(busOptions.UserName);
                        host.Password(busOptions.Password);
                    });


                    cfg.ConfigureEndpoints(ctx);

                });
            });


            return services;
        }

    }
}
