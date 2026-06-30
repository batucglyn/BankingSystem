using Banking.Authentication.Constants;
using Banking.Services.Account.Application.Features.Accounts.GetMyAccounts;
using MediatR;

namespace Banking.Services.Account.Api.Endpoints.Accounts.GetMyAccounts
{
    public static class GetMyAccountsEndpoint
    {
        public static RouteGroupBuilder MapGetMyAccountsEndpoint(
            this RouteGroupBuilder group)
        {
            group.MapGet("/me", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetMyAccountsQuery(),
                    cancellationToken);

                if (!result.IsSuccess)
                    return Results.BadRequest(result);

                return Results.Ok(result);
            })
            .WithName("GetMyAccounts")
            .Produces(200)
            .Produces(400)
            .RequireAuthorization(AuthorizationPolicies.Authenticated);

            return group;
        }
    }
}
