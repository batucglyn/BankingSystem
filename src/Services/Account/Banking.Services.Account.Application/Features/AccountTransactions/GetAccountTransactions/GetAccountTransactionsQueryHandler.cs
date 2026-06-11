using Banking.Services.Account.Application.Abstractions;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.AccountTransactions.GetAccountTransactions
{
    public sealed class GetAccountTransactionsQueryHandler
    : IRequestHandler<
        GetAccountTransactionsQuery,
        Result<List<GetAccountTransactionsResponse>>>
    {
        private readonly IAccountDbContext _context;

        public GetAccountTransactionsQueryHandler(
            IAccountDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<GetAccountTransactionsResponse>>> Handle(
            GetAccountTransactionsQuery request,
            CancellationToken cancellationToken)
        {
            var transactions = await _context.AccountTransactions
                .AsNoTracking()
                .Where(x => x.AccountId == request.AccountId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new GetAccountTransactionsResponse(
                    x.Id,
                    x.AccountId,
                    x.Amount,
                    x.Type,
                    x.Description,
                    x.CreatedAt))
                .ToListAsync(cancellationToken);

            return Result<List<GetAccountTransactionsResponse>>
                .Success(transactions);
        }
    }
}
