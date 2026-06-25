using Banking.Observability.DependencyInjection;
using Banking.Services.Notification.Api;
using Banking.Shared.Correlation;

var builder = WebApplication.CreateBuilder(args);
builder.AddBankingObservability("Notification.Api");
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMassTransitConfiguration(builder.Configuration);

var app = builder.Build();
app.UseCorrelationId();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.Run();

