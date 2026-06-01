


using Banking.Services.Account.Api.Endpoints.Accounts;
using Banking.Services.Account.Application.Features.Accounts.CreateAccount;
using Banking.Services.Account.Infrastructure.Persistence.DependencyInjection;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();


builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateAccountCommandHandler).Assembly);
});

builder.Services.AddValidatorsFromAssembly(
    typeof(CreateAccountCommandValidator).Assembly);





var app = builder.Build();
app.MapAccountEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.Run();


