using Banking.Services.Account.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Banking.Services.Account.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(
                    Assembly.GetExecutingAssembly());
            });

            services.AddValidatorsFromAssembly(
                Assembly.GetExecutingAssembly());
            
            services.AddTransient(
           typeof(IPipelineBehavior<,>),
           typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
