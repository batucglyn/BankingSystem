using Banking.Services.Customer.Application.Features.Customers.GetCustomers;
using MediatR;

namespace Banking.Services.Customer.Api.Endpoints.Customers.GetCustomers
{
    public static class GetCustomersEndpoint
    {
        public static IEndpointRouteBuilder MapGetCustomersEndpoint(
            this IEndpointRouteBuilder group)
        {
            group.MapGet("/", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetCustomersQuery(),
                    cancellationToken);

                return Results.Ok(result);
            })
            .WithName("GetCustomers")
            .Produces(StatusCodes.Status200OK);

            return group;
        }
    }
}
