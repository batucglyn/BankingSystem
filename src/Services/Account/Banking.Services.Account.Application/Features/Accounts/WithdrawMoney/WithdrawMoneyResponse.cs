using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.WithdrawMoney
{
    public sealed record WithdrawMoneyResponse(
      Guid AccountId,
      decimal NewBalance,
      DateTime TransactionDate
  );
}
