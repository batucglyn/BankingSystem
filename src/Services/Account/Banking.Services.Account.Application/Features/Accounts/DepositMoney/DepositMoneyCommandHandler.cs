using Banking.Services.Account.Application.Abstractions;
using Banking.Services.Account.Domain.Entities;
using Banking.Services.Account.Domain.Enums;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.DepositMoney
{
    public sealed class DepositMoneyCommandHandler
      : IRequestHandler<DepositMoneyCommand, Result<DepositMoneyResponse>>
    {
        private readonly IAccountDbContext _context;

        public DepositMoneyCommandHandler(IAccountDbContext context)
        {
            _context = context;
        }

        public async Task<Result<DepositMoneyResponse>> Handle(
            DepositMoneyCommand request,
            CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == request.AccountId, cancellationToken);

            if (account is null)
            {
                return Result<DepositMoneyResponse>.Failure("Account not found.");
            }
            try
            {
                account.Deposit(request.Amount);
            }
            catch (Exception ex)
            {
                return Result<DepositMoneyResponse>.Failure(ex.Message);
            }

            account.Deposit(request.Amount);

            var transaction = new AccountTransaction(
                account.Id,
                request.Amount,
                account.Balance.Currency,
                TransactionType.Deposit,
                "Money deposited.");

            await _context.AccountTransactions.AddAsync(transaction, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<DepositMoneyResponse>.Success(
                new DepositMoneyResponse(
                    account.Id,
                    account.Balance.Amount,
                    DateTime.UtcNow));
        }
    }
}
