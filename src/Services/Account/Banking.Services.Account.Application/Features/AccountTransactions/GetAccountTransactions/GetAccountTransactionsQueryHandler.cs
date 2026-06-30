using Banking.Authentication.CurrentUser;
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
        private readonly ICurrentUser _currentUser;
        private readonly ICustomerServiceClient _customerServiceClient;

        public GetAccountTransactionsQueryHandler(
            IAccountDbContext context,
            ICurrentUser currentUser,
            ICustomerServiceClient customerServiceClient)
        {
            _context = context;
            _currentUser = currentUser;
            _customerServiceClient = customerServiceClient;
        }

        public async Task<Result<List<GetAccountTransactionsResponse>>> Handle(
            GetAccountTransactionsQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated ||
                string.IsNullOrWhiteSpace(_currentUser.UserId))
            {
                return Result<List<GetAccountTransactionsResponse>>
                    .Failure("User is not authenticated.");
            }

            var account = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.AccountId,
                    cancellationToken);

            if (account is null)
            {
                return Result<List<GetAccountTransactionsResponse>>
                    .Failure("Account not found.");
            }

            if (!_currentUser.IsInRole("Admin"))
            {
                var customer = await _customerServiceClient
                    .GetCustomerByKeycloakUserIdAsync(
                        _currentUser.UserId,
                        cancellationToken);

                if (customer is null)
                {
                    return Result<List<GetAccountTransactionsResponse>>
                        .Failure("Customer profile not found.");
                }

                if (!customer.IsActive)
                {
                    return Result<List<GetAccountTransactionsResponse>>
                        .Failure("Customer is not active.");
                }

                if (account.CustomerId != customer.CustomerId)
                {
                    return Result<List<GetAccountTransactionsResponse>>
                        .Failure("Account does not belong to the authenticated customer.");
                }
            }

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
