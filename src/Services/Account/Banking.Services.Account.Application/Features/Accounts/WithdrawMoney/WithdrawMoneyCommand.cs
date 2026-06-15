using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.WithdrawMoney
{
    public sealed record WithdrawMoneyCommand(
     Guid AccountId,
     decimal Amount
 ) : IRequest<Result<WithdrawMoneyResponse>>;
}
