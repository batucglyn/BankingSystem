using Banking.Authentication.DependencyInjection;
using Banking.Observability.DependencyInjection;
using Banking.Shared.Correlation;

var builder = WebApplication.CreateBuilder(args);

builder.AddBankingObservability("Gateway");

builder.Services.AddOpenApi();

builder.Services.AddBankingAuthentication(builder.Configuration);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseCorrelationId();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();