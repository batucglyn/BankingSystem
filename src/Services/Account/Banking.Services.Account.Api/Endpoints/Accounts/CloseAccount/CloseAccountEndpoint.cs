using Banking.Authentication.Constants;
using Banking.Services.Account.Application.Features.Accounts.CloseAccount;
using MediatR;

namespace Banking.Services.Account.Api.Endpoints.Accounts.CloseAccount
{
    public static class CloseAccountEndpoint
    {
        public static RouteGroupBuilder MapCloseAccountEndpoint(
            this RouteGroupBuilder group)
        {
            group.MapPost(
                "/close",
                async (
                    CloseAccountCommand command,
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

                    return Results.Ok();
                })
                 .RequireAuthorization(AuthorizationPolicies.Admin)
                .WithName("CloseAccount");

            return group;
        }
    }
}
