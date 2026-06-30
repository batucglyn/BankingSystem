using Banking.Authentication.Constants;
using Banking.Services.Account.Application.Features.Accounts.BlockAccount;
using MediatR;

namespace Banking.Services.Account.Api.Endpoints.Accounts.BlockAccount
{
    public static class BlockAccountEndpoint
    {
        public static RouteGroupBuilder MapBlockAccountEndpoint(
            this RouteGroupBuilder group)
        {
            group.MapPost(
                "/block",
                async (
                    BlockAccountCommand command,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        command,
                        cancellationToken);

                    if (!result.IsSuccess)
                    {
                        return Results.BadRequest(new
                        {
                            error = result.ErrorMessage
                        });
                    }

                    return Results.Ok();
                })
                .RequireAuthorization(AuthorizationPolicies.Admin)
                .WithName("BlockAccount")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            return group;
        }
    }
}
