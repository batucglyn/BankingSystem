using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.AccountTransactions.GetAccountTransactions
{
    public sealed record GetAccountTransactionsQuery(
    Guid AccountId
) : IRequest<Result<List<GetAccountTransactionsResponse>>>;
}
