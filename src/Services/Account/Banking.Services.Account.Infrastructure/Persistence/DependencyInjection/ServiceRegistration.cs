using Banking.Services.Account.Application.Abstractions;
using Banking.Services.Account.Infrastructure.Persistence.Context;
using Banking.Services.Account.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Banking.Services.Account.Infrastructure.Persistence.DependencyInjection;

public static class ServiceRegistration
{


    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<AccountDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSql")));


        services.AddScoped<IAccountDbContext, AccountDbContext>();
        //degisecek
        services.AddHttpClient<ICustomerServiceClient, CustomerServiceClient>(
    client =>
    {
        client.BaseAddress = new Uri(
            configuration["CustomerService:BaseUrl"]!);
    });
        return services;
    }



}

