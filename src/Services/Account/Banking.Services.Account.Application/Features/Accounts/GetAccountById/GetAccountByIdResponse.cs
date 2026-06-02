using Banking.Services.Account.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.GetAccountById
{
    public sealed record GetAccountByIdResponse(
     Guid Id,
     string AccountNumber,
     string IBAN,
     decimal Balance,
     CurrencyType Currency
 );
}
