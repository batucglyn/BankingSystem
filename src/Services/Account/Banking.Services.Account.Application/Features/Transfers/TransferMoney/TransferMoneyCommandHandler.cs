using Banking.Services.Account.Application.Abstractions;
using Banking.Services.Account.Domain.Entities;
using Banking.Services.Account.Domain.Enums;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Transfers.TransferMoney
{
    public sealed class TransferMoneyCommandHandler
    : IRequestHandler<TransferMoneyCommand, Result<TransferMoneyResponse>>
    {
        private readonly IAccountDbContext _context;

        public TransferMoneyCommandHandler(IAccountDbContext context)
        {
            _context = context;
        }

        public async Task<Result<TransferMoneyResponse>> Handle(
            TransferMoneyCommand request,
            CancellationToken cancellationToken)
        {
            var fromAccount = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == request.FromAccountId, cancellationToken);

            if (fromAccount is null)
            {
                return Result<TransferMoneyResponse>
                    .Failure("Sender account not found.");
            }

            var toAccount = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == request.ToAccountId, cancellationToken);

            if (toAccount is null)
            {
                return Result<TransferMoneyResponse>
                    .Failure("Receiver account not found.");
            }

            try
            {
                fromAccount.Withdraw(request.Amount);
                toAccount.Deposit(request.Amount);
            }
            catch (Exception ex)
            {
                return Result<TransferMoneyResponse>
                    .Failure(ex.Message);
            }

            //account transactions for audit trail

            var transferOutTransaction = new AccountTransaction(
            fromAccount.Id,
            request.Amount,
            fromAccount.Balance.Currency,
            TransactionType.TransferOut,
            $"Transfer to account {toAccount.Id}");

            var transferInTransaction = new AccountTransaction(
                toAccount.Id,
                request.Amount,
                    toAccount.Balance.Currency,
                TransactionType.TransferIn,
                $"Transfer from account {fromAccount.Id}");

            await _context.AccountTransactions.AddAsync(
                transferOutTransaction,
                cancellationToken);

            await _context.AccountTransactions.AddAsync(
                transferInTransaction,
                cancellationToken);


            await _context.SaveChangesAsync(cancellationToken);

            var response = new TransferMoneyResponse(
                fromAccount.Id,
                toAccount.Id,

                request.Amount,
                DateTime.UtcNow);

            return Result<TransferMoneyResponse>.Success(response);
        }
    }
}
