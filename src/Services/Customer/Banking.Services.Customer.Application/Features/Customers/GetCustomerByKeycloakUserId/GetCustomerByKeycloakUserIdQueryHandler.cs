using Banking.Services.Customer.Application.Abstractions;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.GetCustomerByKeycloakUserId
{
    public sealed class GetCustomerByKeycloakUserIdQueryHandler
    : IRequestHandler<GetCustomerByKeycloakUserIdQuery, Result<GetCustomerByKeycloakUserIdResponse>>
    {
        private readonly ICustomerDbContext _context;

        public GetCustomerByKeycloakUserIdQueryHandler(ICustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetCustomerByKeycloakUserIdResponse>> Handle(
            GetCustomerByKeycloakUserIdQuery request,
            CancellationToken cancellationToken)
        {
            var customer = await _context.Customers
                .AsNoTracking()
                .Where(x => x.KeycloakUserId == request.KeycloakUserId)
                .Select(x => new GetCustomerByKeycloakUserIdResponse(
                    x.Id,
                    x.KeycloakUserId,
                    x.IsActive))
                .FirstOrDefaultAsync(cancellationToken);

            if (customer is null)
            {
                return Result<GetCustomerByKeycloakUserIdResponse>
                    .Failure("Customer not found.");
            }

            return Result<GetCustomerByKeycloakUserIdResponse>.Success(customer);
        }
    }
}
