using Banking.Services.Customer.Application.Features.Customers.DeactivateCustomer;
using MediatR;

namespace Banking.Services.Customer.Api.Endpoints.Customers.DeactivateCustomer
{
    public static class DeactivateCustomerEndpoint
    {
        public static IEndpointRouteBuilder MapDeactivateCustomerEndpoint(
            this IEndpointRouteBuilder group)
        {
            group.MapPut("/{id:guid}/deactivate", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new DeactivateCustomerCommand(id),
                    cancellationToken);

                if (!result.IsSuccess)
                    return Results.NotFound(result);

                return Results.Ok(result);
            })
            .WithName("DeactivateCustomer")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}
