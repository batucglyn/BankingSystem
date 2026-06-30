using Banking.Authentication.DependencyInjection;
using Banking.Bus.Extensions;
using Banking.Observability.DependencyInjection;
using Banking.Services.Customer.Api.Endpoints.Customers;
using Banking.Services.Customer.Application;
using Banking.Services.Customer.Infrastructure.DependencyInjection;
using Banking.Shared.Correlation;
using Banking.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.AddBankingObservability("Customer.Api");
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddBankingAuthentication(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddCommonMassTransit(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapCustomerEndpoints();
app.UseCorrelationId();
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Run();

