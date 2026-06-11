using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Transfers.TransferMoney
{
    public sealed record TransferMoneyCommand(
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount
) : IRequest<Result<TransferMoneyResponse>>;
}
