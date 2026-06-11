using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Transfers.TransferMoney
{
    public sealed record TransferMoneyResponse(
     Guid FromAccountId,
     Guid ToAccountId,
     decimal Amount,
     DateTime TransferDate
 );
}
