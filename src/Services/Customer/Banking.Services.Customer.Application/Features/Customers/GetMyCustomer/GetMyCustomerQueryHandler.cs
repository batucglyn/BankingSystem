using Banking.Authentication.CurrentUser;
using Banking.Services.Customer.Application.Abstractions;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.GetMyCustomer
{
    public sealed class GetMyCustomerQueryHandler
    : IRequestHandler<GetMyCustomerQuery, Result<GetMyCustomerResponse>>
    {
        private readonly ICustomerDbContext _context;
        private readonly ICurrentUser _currentUser;

        public GetMyCustomerQueryHandler(
            ICustomerDbContext context,
            ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Result<GetMyCustomerResponse>> Handle(
            GetMyCustomerQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated ||
                string.IsNullOrWhiteSpace(_currentUser.UserId))
            {
                return Result<GetMyCustomerResponse>.Failure("User is not authenticated.");
            }

            var customer = await _context.Customers
                .AsNoTracking()
                .Where(x => x.KeycloakUserId == _currentUser.UserId)
                .Select(x => new GetMyCustomerResponse(
                    x.Id,
                    x.FirstName,
                    x.LastName,
                    x.Email,
                    x.PhoneNumber,
                    x.IdentityNumber,
                    x.PhotoUrl,
                    x.IsActive))
                .FirstOrDefaultAsync(cancellationToken);

            if (customer is null)
            {
                return Result<GetMyCustomerResponse>.Failure("Customer profile not found.");
            }

            return Result<GetMyCustomerResponse>.Success(customer);
        }
    }
}
