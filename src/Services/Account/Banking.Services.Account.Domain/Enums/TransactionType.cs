using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Domain.Enums
{
    public enum TransactionType
    {
        Deposit = 1,
        Withdraw = 2,
        TransferIn = 3,
        TransferOut = 4
    }
}
