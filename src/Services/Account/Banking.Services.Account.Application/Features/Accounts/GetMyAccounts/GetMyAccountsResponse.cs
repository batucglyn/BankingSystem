using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.GetMyAccounts
{
    public sealed record GetMyAccountsResponse(
    Guid Id,
    string AccountNumber,
    string Iban,
    decimal Balance,
    int Currency,
    int Status);
}
