using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.DepositMoney
{
    public sealed record DepositMoneyResponse(
     Guid AccountId,
     decimal NewBalance,
     DateTime TransactionDate
 );
}
