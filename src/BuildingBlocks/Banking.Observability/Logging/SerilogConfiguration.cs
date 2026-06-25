using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Observability.Logging
{

    public static class SerilogConfiguration
    {
        public static LoggerConfiguration ConfigureBankingSerilog(
            this LoggerConfiguration loggerConfiguration,
            string serviceName)
        {
            return loggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("ServiceName", serviceName)
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] " +
                    "Service={ServiceName} " +
                    "CorrelationId={CorrelationId} " +
                    "{Message:lj}{NewLine}{Exception}");
        }
    }
}
