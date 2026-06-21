using Banking.Services.Customer.Application.Features.Customers.CheckCustomerExists;
using MediatR;

namespace Banking.Services.Customer.Api.Endpoints.Customers.CheckCustomerExists
{
    public static class CheckCustomerExistsEndpoint
    {
        public static IEndpointRouteBuilder MapCheckCustomerExistsEndpoint(
            this IEndpointRouteBuilder group)
        {
            group.MapGet("/{id:guid}/exists", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new CheckCustomerExistsQuery(id),
                    cancellationToken);

                return Results.Ok(result);
            })
            .WithName("CheckCustomerExists")
            .Produces(StatusCodes.Status200OK);

            return group;
        }
    }
}
