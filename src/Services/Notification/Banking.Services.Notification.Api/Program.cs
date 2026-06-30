using Banking.Observability.DependencyInjection;
using Banking.Services.Notification.Api;
using Banking.Services.Notification.Api.Options;
using Banking.Services.Notification.Api.Services;
using Banking.Shared.Correlation;

var builder = WebApplication.CreateBuilder(args);
builder.AddBankingObservability("Notification.Api");

builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection("Email"));

builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();







// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMassTransitConfiguration(builder.Configuration);

var app = builder.Build();
app.UseCorrelationId();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.Run();

