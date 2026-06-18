using Banking.Services.Customer.Api.Endpoints.Customers;
using Banking.Services.Customer.Application;
using Banking.Services.Customer.Infrastructure.DependencyInjection;
using Banking.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddApplicationServices();



var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapCustomerEndpoints();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Run();

