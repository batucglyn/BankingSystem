using Banking.Services.Account.Application.Features.Accounts.CreateAccount;
using MediatR;

namespace Banking.Services.Account.Api.Endpoints.Accounts
{
    public static class AccountEndpoints
    {
        public static RouteGroupBuilder MapAccountEndpoints(
       this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/accounts")
                .WithTags("Accounts");

            group.MapPost(
                "/",
                async (
                    CreateAccountCommand command,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var response = await sender.Send(
                        command,
                        cancellationToken);

                    return Results.Ok(response);
                })
                .WithName("CreateAccount")
                .Produces<CreateAccountResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            return group;
        }



    }
}
