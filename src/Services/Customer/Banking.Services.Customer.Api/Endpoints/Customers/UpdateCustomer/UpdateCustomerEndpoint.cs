using MediatR;

namespace Banking.Services.Customer.Api.Endpoints.Customers.UpdateCustomer
{
    public static class UpdateCustomerEndpoint
    {
        public static IEndpointRouteBuilder MapUpdateCustomerEndpoint(
            this IEndpointRouteBuilder group)
        {
            group.MapPut("/{id:guid}", async (
                Guid id,
                UpdateCustomerCommand request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = request with
                {
                    CustomerId = id
                };

                var result = await sender.Send(
                    command,
                    cancellationToken);

                if (!result.IsSuccess)
                    return Results.BadRequest(result);

                return Results.Ok(result);
            })
            .WithName("UpdateCustomer")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            return group;
        }
    }
}
