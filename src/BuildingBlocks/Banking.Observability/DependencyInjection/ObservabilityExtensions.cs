using Banking.Observability.Logging;
using Microsoft.AspNetCore.Builder;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Observability.DependencyInjection
{
    public static class ObservabilityExtensions
    {
        public static WebApplicationBuilder AddBankingObservability(
            this WebApplicationBuilder builder,
            string serviceName)
        {
            Log.Logger = new LoggerConfiguration()
                .ConfigureBankingSerilog(serviceName)
                .CreateLogger();

            builder.Host.UseSerilog();

            return builder;
        }
    }
}
