using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.CloseAccount
{
    public sealed record CloseAccountCommand(
      Guid AccountId
  ) : IRequest<Result>;
}
