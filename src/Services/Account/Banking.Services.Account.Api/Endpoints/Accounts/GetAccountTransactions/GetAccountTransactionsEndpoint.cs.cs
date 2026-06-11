using Banking.Services.Account.Application.Features.AccountTransactions.GetAccountTransactions;
using MediatR;

namespace Banking.Services.Account.Api.Endpoints.Accounts.GetAccountTransactions
{
    public static class GetAccountTransactionsEndpoint
    {
        public static RouteGroupBuilder MapGetAccountTransactionsEndpoint(
            this RouteGroupBuilder group)
        {
            group.MapGet(
                "/{id:guid}/transactions",
                async (
                    Guid id,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new GetAccountTransactionsQuery(id),
                        cancellationToken);

                    if (!result.IsSuccess)
                    {
                        return Results.BadRequest(
                            new
                            {
                                error = result.ErrorMessage
                            });
                    }

                    return Results.Ok(result.Data);
                })
                .WithName("GetAccountTransactions")
                .Produces<List<GetAccountTransactionsResponse>>(
                    StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            return group;
        }
    }
}
