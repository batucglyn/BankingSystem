using Banking.Authentication.CurrentUser;
using Banking.Services.Account.Application.Abstractions;
using Banking.Services.Account.Application.Features.Transfers.TransferMoney;
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
        private readonly ICurrentUser _currentUser;
        private readonly ICustomerServiceClient _customerServiceClient;
        public WithdrawMoneyCommandHandler(IAccountDbContext context, ICurrentUser currentUser, ICustomerServiceClient customerServiceClient)
        {
            _context = context;
            _currentUser = currentUser;
            _customerServiceClient = customerServiceClient;
        }

        public async Task<Result<WithdrawMoneyResponse>> Handle(
            WithdrawMoneyCommand request,
            CancellationToken cancellationToken)
        {


            if (request.Amount <= 0)
            {

                return Result<WithdrawMoneyResponse>
                    .Failure("Withdrawal amount must be greater than zero.");
            }





            if (!_currentUser.IsAuthenticated ||
           string.IsNullOrWhiteSpace(_currentUser.UserId))
            {
                return Result<WithdrawMoneyResponse>
                    .Failure("User is not authenticated.");
            }

            var customer = await _customerServiceClient
               .GetCustomerByKeycloakUserIdAsync(
                   _currentUser.UserId,
                   cancellationToken);

            if (customer is null)
            {
                return Result<WithdrawMoneyResponse>
                    .Failure("Customer profile not found.");
            }

            if (!customer.IsActive)
            {
                return Result<WithdrawMoneyResponse>
                    .Failure("Customer is not active.");
            }




            var account = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == request.AccountId, cancellationToken);

            if (account is null)
            {
                return Result<WithdrawMoneyResponse>.Failure("Account not found.");
            }

            if(account.Status != AccountStatus.Active)
            {
                return Result<WithdrawMoneyResponse>.Failure("Account is not active.");
            }


            if (account.CustomerId != customer.CustomerId)
            {
                return Result<WithdrawMoneyResponse>.Failure("Account does not belong to the authenticated customer.");
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
