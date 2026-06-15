using Banking.Services.Account.Application.Abstractions;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.CloseAccount
{
    public sealed class CloseAccountCommandHandler
    : IRequestHandler<CloseAccountCommand, Result>
    {
        private readonly IAccountDbContext _context;

        public CloseAccountCommandHandler(
            IAccountDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(
            CloseAccountCommand request,
            CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(
                    x => x.Id == request.AccountId,
                    cancellationToken);

            if (account is null)
            {
                return Result.Failure(
                    "Account not found.");
            }

            try
            {
                account.Close();
            }
            catch (Exception ex)
            {
                return Result.Failure(
                    ex.Message);
            }

            await _context.SaveChangesAsync(
                cancellationToken);

            return Result.Success();
        }
    }
}
