using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.GetAccountById
{
    public sealed record GetAccountByIdQuery(
      Guid Id
  ) : IRequest<Result<GetAccountByIdResponse>>;
}
