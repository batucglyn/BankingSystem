using Banking.Authentication.Constants;
using Banking.Services.Customer.Application.Features.Customers.GetCustomerByKeycloakUserId;
using MediatR;

namespace Banking.Services.Customer.Api.Endpoints.Customers.GetCustomerByKeycloakId
{
    public static class GetCustomerByKeycloakUserIdEndpoint
    {
        public static RouteGroupBuilder MapGetCustomerByKeycloakUserIdEndpoint(
            this RouteGroupBuilder group)
        {
            group.MapGet("/by-keycloak-user/{keycloakUserId}", async (
                string keycloakUserId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetCustomerByKeycloakUserIdQuery(keycloakUserId),
                    cancellationToken);

                if (!result.IsSuccess)
                    return Results.NotFound(result);

                return Results.Ok(result);
            })
            .WithName("GetCustomerByKeycloakUserId")
            .Produces(200)
            .Produces(404)
            .RequireAuthorization(AuthorizationPolicies.Authenticated);

            return group;
        }
    }
}
