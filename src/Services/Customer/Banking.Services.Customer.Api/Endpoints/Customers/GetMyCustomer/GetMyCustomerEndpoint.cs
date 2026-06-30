using Banking.Authentication.Constants;
using Banking.Services.Customer.Application.Features.Customers.GetMyCustomer;
using MediatR;

namespace Banking.Services.Customer.Api.Endpoints.Customers.GetMyCustomer
{
    public static class GetMyCustomerEndpoint
    {
        public static RouteGroupBuilder MapGetMyCustomerEndpoint(
            this RouteGroupBuilder group)
        {
            group.MapGet("/me", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetMyCustomerQuery(),
                    cancellationToken);

                if (!result.IsSuccess)
                    return Results.BadRequest(result);

                return Results.Ok(result);
            })
            .WithName("GetMyCustomer")
            .Produces(200)
            .Produces(400)
            .RequireAuthorization(AuthorizationPolicies.Authenticated);

            return group;
        }
    }
}
