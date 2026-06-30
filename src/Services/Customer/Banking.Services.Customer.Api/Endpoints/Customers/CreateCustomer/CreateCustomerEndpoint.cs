using Banking.Authentication.Constants;
using Banking.Services.Customer.Application.Features.Customers.CreateCustomer;
using MediatR;

namespace Banking.Services.Customer.Api.Endpoints.Customers.CreateCustomer
{
    public static class CreateCustomerEndpoint
    {
        public static IEndpointRouteBuilder MapCreateCustomerEndpoint(
    this IEndpointRouteBuilder group)
        {
            group.MapPost("/", async (
                CreateCustomerCommand command,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);

                if (!result.IsSuccess)
                    return Results.BadRequest(result);

                return Results.Ok(result);
            })
                .RequireAuthorization(AuthorizationPolicies.Admin)
            .WithName("CreateCustomer")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            return group;
        }
    }
}
