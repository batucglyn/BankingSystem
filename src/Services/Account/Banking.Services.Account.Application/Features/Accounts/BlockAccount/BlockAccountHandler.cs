using Banking.Services.Account.Application.Abstractions;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.BlockAccount
{

    public sealed class BlockAccountCommandHandler
        : IRequestHandler<BlockAccountCommand, Result>
    {
        private readonly IAccountDbContext _context;

        public BlockAccountCommandHandler(
            IAccountDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(
            BlockAccountCommand request,
            CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(
                    x => x.Id == request.AccountId,
                    cancellationToken);

            if (account is null)
            {
                return Result.Failure(
                    "Account not found");
            }

            account.Block();

            await _context.SaveChangesAsync(
                cancellationToken);

            return Result.Success();
        }
    }
}
