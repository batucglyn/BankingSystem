using Banking.Authentication.Constants;
using Banking.Services.Account.Api.Endpoints.Accounts.BlockAccount;
using Banking.Services.Account.Api.Endpoints.Accounts.CloseAccount;
using Banking.Services.Account.Api.Endpoints.Accounts.CreateAccount;
using Banking.Services.Account.Api.Endpoints.Accounts.DepositMoney;
using Banking.Services.Account.Api.Endpoints.Accounts.GetAccountById;
using Banking.Services.Account.Api.Endpoints.Accounts.GetAccountTransactions;
using Banking.Services.Account.Api.Endpoints.Accounts.TransferMoney;
using Banking.Services.Account.Api.Endpoints.Accounts.WithdrawMoney;
using Banking.Services.Account.Application.Features.Accounts.CreateAccount;
using Banking.Services.Account.Application.Features.Accounts.GetAccountById;
using MediatR;

namespace Banking.Services.Account.Api.Endpoints.Accounts
{
    public static class AccountEndpoints
    {
        public static RouteGroupBuilder MapAccountEndpoints(
         this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/accounts")
                .WithTags("Accounts")
                  .RequireAuthorization(AuthorizationPolicies.Authenticated);
            group.MapCreateAccountEndpoint();

            group.MapGetAccountByIdEndpoint();

            group.MapTransferMoneyEndpoint();
            group.MapGetAccountTransactionsEndpoint();
            group.MapDepositMoneyEndpoint();

            group.MapWithdrawMoneyEndpoint();
            group.MapBlockAccountEndpoint();
            group.MapCloseAccountEndpoint();
            return group;
        }
    }
}
