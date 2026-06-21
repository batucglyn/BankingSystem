using Banking.Services.Customer.Application.Abstractions;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.DeactivateCustomer
{
    public sealed class DeactivateCustomerCommandHandler
     : IRequestHandler<DeactivateCustomerCommand, Result>
    {
        private readonly ICustomerDbContext _context;

        public DeactivateCustomerCommandHandler(ICustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(
            DeactivateCustomerCommand request,
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

            customer.Deactivate();

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
