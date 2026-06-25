using Banking.Observability.DependencyInjection;
using Banking.Shared.Correlation;

var builder = WebApplication.CreateBuilder(args);
builder.AddBankingObservability("Gateway");



builder.Services.AddOpenApi();

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();


app.MapReverseProxy();
app.UseCorrelationId();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.Run();

