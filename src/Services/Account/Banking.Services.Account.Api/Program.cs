


using Banking.Bus.Extensions;
using Banking.Observability.DependencyInjection;
using Banking.Services.Account.Api.Endpoints.Accounts;
using Banking.Services.Account.Application;
using Banking.Services.Account.Application.Behaviors;
using Banking.Services.Account.Application.Features.Accounts.CreateAccount;
using Banking.Services.Account.Infrastructure.DependencyInjection;
using Banking.Shared.Correlation;
using Banking.Shared.Middlewares;
using FluentValidation;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.AddBankingObservability("Account.Api");
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddCommonMassTransit(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplicationServices();




var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapAccountEndpoints();
app.UseCorrelationId();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.Run();


