using Banking.Services.Customer.Application.Abstractions;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.CheckCustomerExists
{
    public sealed class CheckCustomerExistsQueryHandler
      : IRequestHandler<CheckCustomerExistsQuery, Result<CheckCustomerExistsResponse>>
    {
        private readonly ICustomerDbContext _context;

        public CheckCustomerExistsQueryHandler(ICustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CheckCustomerExistsResponse>> Handle(
            CheckCustomerExistsQuery request,
            CancellationToken cancellationToken)
        {
            var customer = await _context.Customers
                .AsNoTracking()
                .Where(x => x.Id == request.CustomerId)
                .Select(x => new CheckCustomerExistsResponse(
                    true,
                    x.IsActive))
                .FirstOrDefaultAsync(cancellationToken);

            if (customer is null)
            {
                return Result<CheckCustomerExistsResponse>.Success(
                    new CheckCustomerExistsResponse(false, false));
            }

            return Result<CheckCustomerExistsResponse>.Success(customer);
        }
    }
}
