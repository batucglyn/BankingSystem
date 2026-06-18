using Banking.Services.Customer.Application.Features.Customers.GetCustomerById;
using MediatR;

namespace Banking.Services.Customer.Api.Endpoints.Customers.GetCustomer
{
    public static class GetCustomerByIdEndpoint
    {
        public static IEndpointRouteBuilder MapGetCustomerByIdEndpoint(
            this IEndpointRouteBuilder group)
        {
            group.MapGet("/{id:guid}", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetCustomerByIdQuery(id),
                    cancellationToken);

                if (!result.IsSuccess)
                    return Results.NotFound(result);

                return Results.Ok(result);
            })
            .WithName("GetCustomerById")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}
