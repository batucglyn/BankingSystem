using Banking.Services.Customer.Application.Abstractions;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.ActivateCustomer
{
    public sealed class ActivateCustomerCommandHandler
    : IRequestHandler<ActivateCustomerCommand, Result>
    {
        private readonly ICustomerDbContext _context;

        public ActivateCustomerCommandHandler(ICustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(
            ActivateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(
                    x => x.Id == request.CustomerId,
                    cancellationToken);

            if (customer is null)
            {
                return Result.Failure("Customer not found.");
            }

            customer.Activate();

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
