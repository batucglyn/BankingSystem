using Banking.Services.Account.Application.Features.Accounts.DepositMoney;
using MediatR;

namespace Banking.Services.Account.Api.Endpoints.Accounts.DepositMoney
{
    public static class DepositMoneyEndpoint
    {
        public static RouteGroupBuilder MapDepositMoneyEndpoint(
            this RouteGroupBuilder group)
        {
            group.MapPost(
                "/deposit",
                async (
                    DepositMoneyCommand command,
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
                .WithName("DepositMoney")
                .Produces<DepositMoneyResponse>(
                    StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            return group;
        }
    }
}
