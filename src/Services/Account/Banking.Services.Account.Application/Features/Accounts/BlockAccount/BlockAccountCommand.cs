using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.BlockAccount
{
    public sealed record BlockAccountCommand(
      Guid AccountId
  ) : IRequest<Result>;
}
