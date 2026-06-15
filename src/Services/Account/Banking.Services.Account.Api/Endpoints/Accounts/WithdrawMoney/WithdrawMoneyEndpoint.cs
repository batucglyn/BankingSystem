using Banking.Services.Account.Application.Features.Accounts.WithdrawMoney;
using MediatR;

namespace Banking.Services.Account.Api.Endpoints.Accounts.WithdrawMoney
{
    public static class WithdrawMoneyEndpoint
    {
        public static RouteGroupBuilder MapWithdrawMoneyEndpoint(
            this RouteGroupBuilder group)
        {
            group.MapPost(
                "/withdraw",
                async (
                    WithdrawMoneyCommand command,
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
                .WithName("WithdrawMoney")
                .Produces<WithdrawMoneyResponse>(
                    StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            return group;
        }
    }
}
