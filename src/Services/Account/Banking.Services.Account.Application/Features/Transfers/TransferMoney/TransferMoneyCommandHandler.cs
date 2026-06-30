using Banking.Authentication.CurrentUser;
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
        private readonly ICurrentUser _currentUser;
        private readonly ICustomerServiceClient _customerServiceClient;
        public TransferMoneyCommandHandler(IAccountDbContext context, ICurrentUser currentUser, ICustomerServiceClient customerServiceClient)
        {
            _context = context;
            _currentUser = currentUser;
            _customerServiceClient = customerServiceClient;
        }

        public async Task<Result<TransferMoneyResponse>> Handle(
            TransferMoneyCommand request,
            CancellationToken cancellationToken)
        {


            if (request.Amount <= 0)
            {

                return Result<TransferMoneyResponse>
                    .Failure("Transfer amount must be greater than zero.");
            }

            if (request.FromAccountId == request.ToAccountId)
            {

                return Result<TransferMoneyResponse>
                    .Failure("Sender and receiver accounts must be different.");
            }


            if (!_currentUser.IsAuthenticated ||
           string.IsNullOrWhiteSpace(_currentUser.UserId))
            {
                return Result<TransferMoneyResponse>
                    .Failure("User is not authenticated.");
            }

            var customer = await _customerServiceClient
                .GetCustomerByKeycloakUserIdAsync(
                    _currentUser.UserId,
                    cancellationToken);

            if (customer is null)
            {
                return Result<TransferMoneyResponse>
                    .Failure("Customer profile not found.");
            }

            if (!customer.IsActive)
            {
                return Result<TransferMoneyResponse>
                    .Failure("Customer is not active.");
            }




            var fromAccount = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == request.FromAccountId, cancellationToken);

            if (fromAccount is null)
            {
                return Result<TransferMoneyResponse>
                    .Failure("Sender account not found.");
            }

            if (fromAccount.CustomerId != customer.CustomerId)
            {
                return Result<TransferMoneyResponse>
                    .Failure("You are not allowed to transfer money from this account.");
            }
            if (fromAccount.Status != AccountStatus.Active)
            {
                return Result<TransferMoneyResponse>
                    .Failure("Sender account is not active.");
            }

            var toAccount = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == request.ToAccountId, cancellationToken);

            if (toAccount is null)
            {
                return Result<TransferMoneyResponse>
                    .Failure("Receiver account not found.");
            }
            if (toAccount.Status != AccountStatus.Active)
            {
                return Result<TransferMoneyResponse>
                    .Failure("Receiver account is not active.");
            }

            if (fromAccount.Balance.Currency != toAccount.Balance.Currency)
            {
                return Result<TransferMoneyResponse>
                    .Failure("Cannot transfer money between accounts with different currencies.");
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
