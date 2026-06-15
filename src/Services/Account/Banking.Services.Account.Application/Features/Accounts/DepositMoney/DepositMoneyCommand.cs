using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.DepositMoney
{
    public sealed record DepositMoneyCommand(
     Guid AccountId,
     decimal Amount
 ) : IRequest<Result<DepositMoneyResponse>>;
}
