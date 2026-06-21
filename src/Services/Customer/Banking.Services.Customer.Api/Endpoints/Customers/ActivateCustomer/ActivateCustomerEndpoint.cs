using Banking.Services.Customer.Application.Features.Customers.ActivateCustomer;
using MediatR;

namespace Banking.Services.Customer.Api.Endpoints.Customers.ActivateCustomer
{
    public static class ActivateCustomerEndpoint
    {
        public static IEndpointRouteBuilder MapActivateCustomerEndpoint(
            this IEndpointRouteBuilder group)
        {
            group.MapPut("/{id:guid}/activate", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new ActivateCustomerCommand(id),
                    cancellationToken);

                if (!result.IsSuccess)
                    return Results.NotFound(result);

                return Results.Ok(result);
            })
            .WithName("ActivateCustomer")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}
