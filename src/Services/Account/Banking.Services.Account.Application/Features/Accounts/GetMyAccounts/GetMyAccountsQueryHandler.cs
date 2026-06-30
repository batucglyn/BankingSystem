using Banking.Authentication.CurrentUser;
using Banking.Services.Account.Application.Abstractions;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.GetMyAccounts
{
    public sealed class GetMyAccountsQueryHandler
       : IRequestHandler<GetMyAccountsQuery, Result<IReadOnlyList<GetMyAccountsResponse>>>
    {
        private readonly IAccountDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly ICustomerServiceClient _customerServiceClient;

        public GetMyAccountsQueryHandler(
            IAccountDbContext context,
            ICurrentUser currentUser,
            ICustomerServiceClient customerServiceClient)
        {
            _context = context;
            _currentUser = currentUser;
            _customerServiceClient = customerServiceClient;
        }

        public async Task<Result<IReadOnlyList<GetMyAccountsResponse>>> Handle(
            GetMyAccountsQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated ||
                string.IsNullOrWhiteSpace(_currentUser.UserId))
            {
                return Result<IReadOnlyList<GetMyAccountsResponse>>
                    .Failure("User is not authenticated.");
            }

            var customer = await _customerServiceClient
                .GetCustomerByKeycloakUserIdAsync(
                    _currentUser.UserId,
                    cancellationToken);

            if (customer is null)
            {
                return Result<IReadOnlyList<GetMyAccountsResponse>>
                    .Failure("Customer profile not found.");
            }

            if (!customer.IsActive)
            {
                return Result<IReadOnlyList<GetMyAccountsResponse>>
                    .Failure("Customer is not active.");
            }

            var accounts = await _context.Accounts
                .AsNoTracking()
                .Where(x => x.CustomerId == customer.CustomerId)
                .Select(x => new GetMyAccountsResponse(
                    x.Id,
                    x.AccountNumber,
                    x.IBAN,
                    x.Balance.Amount,
                    (int)x.Balance.Currency,
                    (int)x.Status))
                .ToListAsync(cancellationToken);

            return Result<IReadOnlyList<GetMyAccountsResponse>>
                .Success(accounts);
        }
    }
}
