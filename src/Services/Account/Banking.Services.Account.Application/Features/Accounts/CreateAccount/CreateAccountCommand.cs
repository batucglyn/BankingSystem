using Banking.Services.Account.Domain.Enums;
using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.CreateAccount;

public record CreateAccountCommand( CurrencyType Currency) : IRequest<Result<CreateAccountResponse>>;



