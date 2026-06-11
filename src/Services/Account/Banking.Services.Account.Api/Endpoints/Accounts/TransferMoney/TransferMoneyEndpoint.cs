using Banking.Services.Account.Application.Features.Transfers.TransferMoney;
using MediatR;

namespace Banking.Services.Account.Api.Endpoints.Accounts.TransferMoney
{
    public static class TransferMoneyEndpoint
    {
        public static RouteGroupBuilder MapTransferMoneyEndpoint(
        this RouteGroupBuilder group)
        {
            group.MapPost(
                "/transfer",
                async (
                    TransferMoneyCommand command,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        command,
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
                .WithName("TransferMoney")
                .Produces<TransferMoneyResponse>(
                    StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            return group;
        }
    }
}
