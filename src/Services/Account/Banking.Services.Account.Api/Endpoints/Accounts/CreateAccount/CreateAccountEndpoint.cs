using Banking.Services.Account.Application.Features.Accounts.CreateAccount;
using MediatR;

namespace Banking.Services.Account.Api.Endpoints.Accounts.CreateAccount
{
    public static class CreateAccountEndpoint
    {
        public static RouteGroupBuilder MapCreateAccountEndpoint(
            this RouteGroupBuilder group)
        {
            group.MapPost(
                "/",
                async (
                    CreateAccountCommand command,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        command,
                        cancellationToken);

                    if (!result.IsSuccess)
                    {
                        return Results.BadRequest(
                            new
                            {
                                error = result.ErrorMessage
                            });
                    }

                    return Results.Ok(result.Data);
                })
                .WithName("CreateAccount")
                .Produces<CreateAccountResponse>(
                    StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            return group;
        }
    }
}
