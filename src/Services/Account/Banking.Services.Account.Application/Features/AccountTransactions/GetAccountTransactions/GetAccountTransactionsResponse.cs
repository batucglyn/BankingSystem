using Banking.Services.Account.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.AccountTransactions.GetAccountTransactions
{
    public sealed record GetAccountTransactionsResponse(
     Guid Id,
     Guid AccountId,
     decimal Amount,
     TransactionType Type,
     string Description,
     DateTime CreatedAt
 );
}
