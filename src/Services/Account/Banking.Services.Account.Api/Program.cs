


using Banking.Services.Account.Api.Endpoints.Accounts;
using Banking.Services.Account.Application.Behaviors;
using Banking.Services.Account.Application.Features.Accounts.CreateAccount;
using Banking.Services.Account.Infrastructure.Persistence.DependencyInjection;
using Banking.Shared.Middlewares;
using FluentValidation;
using MediatR;

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

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));



var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapAccountEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.Run();


