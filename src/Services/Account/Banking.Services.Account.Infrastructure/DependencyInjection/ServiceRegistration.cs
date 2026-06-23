using Banking.Services.Account.Application.Abstractions;
using Banking.Services.Account.Infrastructure.Persistence.Context;
using Banking.Services.Account.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Banking.Services.Account.Infrastructure.DependencyInjection;

public static class ServiceRegistration
{


    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<AccountDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSql")));


        services.AddScoped<IAccountDbContext, AccountDbContext>();
        //degisecek
        services.AddHttpClient<ICustomerServiceClient, CustomerServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["CustomerService:BaseUrl"]!);
        
        })
    .AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(
            3,
            retryAttempt => TimeSpan.FromSeconds(2),
            (result, timeSpan, retryCount, context) =>
            {
                Console.WriteLine($"Retry #{retryCount} after {timeSpan.TotalSeconds} seconds");
            }))
    .AddTransientHttpErrorPolicy(policy =>
        policy.CircuitBreakerAsync(
            5,
            TimeSpan.FromSeconds(30)));
        return services;
    }



}

