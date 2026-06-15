using Banking.Services.Account.Application.Abstractions;
using Banking.Services.Account.Domain.Entities;
using Banking.Services.Account.Domain.Enums;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.WithdrawMoney
{
    public sealed class WithdrawMoneyCommandHandler
     : IRequestHandler<WithdrawMoneyCommand, Result<WithdrawMoneyResponse>>
    {
        private readonly IAccountDbContext _context;

        public WithdrawMoneyCommandHandler(IAccountDbContext context)
        {
            _context = context;
        }

        public async Task<Result<WithdrawMoneyResponse>> Handle(
            WithdrawMoneyCommand request,
            CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == request.AccountId, cancellationToken);

            if (account is null)
            {
                return Result<WithdrawMoneyResponse>.Failure("Account not found.");
            }          

            try
            {
                account.Withdraw(request.Amount);
            }
            catch (InvalidOperationException ex)
            {
                return Result<WithdrawMoneyResponse>.Failure(ex.Message);
            }

            var transaction = new AccountTransaction(
                account.Id,
                request.Amount,
                account.Balance.Currency,
                TransactionType.Withdraw,
                "Money withdrawn.");

            await _context.AccountTransactions.AddAsync(transaction, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<WithdrawMoneyResponse>.Success(
                new WithdrawMoneyResponse(
                    account.Id,
                    account.Balance.Amount,
                    DateTime.UtcNow));
        }
    }
}
