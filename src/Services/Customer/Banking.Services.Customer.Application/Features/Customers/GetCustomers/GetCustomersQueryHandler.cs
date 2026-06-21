using Banking.Services.Customer.Application.Abstractions;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.GetCustomers
{
    public sealed class GetCustomersQueryHandler
    : IRequestHandler<GetCustomersQuery, Result<List<GetCustomersResponse>>>
    {
        private readonly ICustomerDbContext _context;

        public GetCustomersQueryHandler(ICustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<GetCustomersResponse>>> Handle(
            GetCustomersQuery request,
            CancellationToken cancellationToken)
        {
            var customers = await _context.Customers
                .AsNoTracking()
                .Select(x => new GetCustomersResponse(
                    x.Id,
                    x.FirstName,
                    x.LastName,
                    x.Email,
                    x.PhoneNumber,
                    x.IsActive))
                .ToListAsync(cancellationToken);

            return Result<List<GetCustomersResponse>>
                .Success(customers);
        }
    }
}
