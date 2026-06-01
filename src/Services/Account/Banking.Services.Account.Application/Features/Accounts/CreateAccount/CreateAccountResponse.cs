using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.CreateAccount;

public sealed record CreateAccountResponse
(Guid Id,
    string AccountNumber,
    string IBAN
);

