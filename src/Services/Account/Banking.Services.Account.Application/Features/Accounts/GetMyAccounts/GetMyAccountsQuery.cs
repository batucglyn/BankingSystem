using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.GetMyAccounts
{
    public sealed record GetMyAccountsQuery
     : IRequest<Result<IReadOnlyList<GetMyAccountsResponse>>>;
}
