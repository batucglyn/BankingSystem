using Banking.Services.Account.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.CreateAccount;

public record CreateAccountCommand(Guid CustomerId, CurrencyType Currency) : IRequest<CreateAccountResponse>;



