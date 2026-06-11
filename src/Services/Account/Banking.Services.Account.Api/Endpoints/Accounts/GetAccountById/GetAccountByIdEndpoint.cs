using Banking.Services.Account.Application.Features.Accounts.GetAccountById;
using MediatR;

namespace Banking.Services.Account.Api.Endpoints.Accounts.GetAccountById
{
    public static class GetAccountByIdEndpoint
    {
        public static RouteGroupBuilder MapGetAccountByIdEndpoint(
            this RouteGroupBuilder group)
        {
            group.MapGet(
                "/{id:guid}",
                async (
                    Guid id,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new GetAccountByIdQuery(id),
                        cancellationToken);

                    if (!result.IsSuccess)
                    {
                        return Results.NotFound(
                            new
                            {
                                error = result.ErrorMessage
                            });
                    }

                    return Results.Ok(result.Data);
                })
                .WithName("GetAccountById")
                .Produces<GetAccountByIdResponse>(
                    StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}
