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

namespace Banking.Services.Account.Application.Features.Accounts.DepositMoney
{
    public sealed class DepositMoneyCommandHandler
       : IRequestHandler<DepositMoneyCommand, Result<DepositMoneyResponse>>
    {
        private readonly IAccountDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly ICustomerServiceClient _customerServiceClient;

        public DepositMoneyCommandHandler(
            IAccountDbContext context,
            ICurrentUser currentUser,
            ICustomerServiceClient customerServiceClient)
        {
            _context = context;
            _currentUser = currentUser;
            _customerServiceClient = customerServiceClient;
        }

        public async Task<Result<DepositMoneyResponse>> Handle(
            DepositMoneyCommand request,
            CancellationToken cancellationToken)
        {

            if (request.Amount <= 0)
            {
                return Result<DepositMoneyResponse>
                    .Failure("Deposit amount must be greater than zero.");

            }


            if (!_currentUser.IsAuthenticated ||
                string.IsNullOrWhiteSpace(_currentUser.UserId))
            {
                return Result<DepositMoneyResponse>
                    .Failure("User is not authenticated.");
            }

            var customer = await _customerServiceClient
                .GetCustomerByKeycloakUserIdAsync(
                    _currentUser.UserId,
                    cancellationToken);

            if (customer is null)
            {
                return Result<DepositMoneyResponse>
                    .Failure("Customer profile not found.");
            }

            if (!customer.IsActive)
            {
                return Result<DepositMoneyResponse>
                    .Failure("Customer is not active.");
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(
                    x => x.Id == request.AccountId,
                    cancellationToken);

            if (account is null)
            {
                return Result<DepositMoneyResponse>
                    .Failure("Account not found.");
            }

            if (account.Status != AccountStatus.Active)
            {

                return Result<DepositMoneyResponse>
                    .Failure("Account is not active.");
            }


            if (account.CustomerId != customer.CustomerId)
            {
                return Result<DepositMoneyResponse>
                    .Failure("Account does not belong to the authenticated customer.");
            }

            try
            {
                account.Deposit(request.Amount);
            }
            catch (InvalidOperationException ex)
            {
                return Result<DepositMoneyResponse>
                    .Failure(ex.Message);
            }

            var transaction = new AccountTransaction(
                account.Id,
                request.Amount,
                account.Balance.Currency,
                TransactionType.Deposit,
                "Money deposited.");

            await _context.AccountTransactions.AddAsync(
                transaction,
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<DepositMoneyResponse>.Success(
                new DepositMoneyResponse(
                    account.Id,
                    account.Balance.Amount,
                    DateTime.UtcNow));
        }
    }
}
