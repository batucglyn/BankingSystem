using Banking.Services.Customer.Application.Abstractions;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Customer.Api.Endpoints.Customers.UpdateCustomer
{
    public sealed class UpdateCustomerCommandHandler
       : IRequestHandler<UpdateCustomerCommand, Result>
    {
        private readonly ICustomerDbContext _context;

        public UpdateCustomerCommandHandler(ICustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(
            UpdateCustomerCommand request,
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

            var emailExists = await _context.Customers
                .AnyAsync(
                    x => x.Email == request.Email &&
                         x.Id != request.CustomerId,
                    cancellationToken);

            if (emailExists)
            {
                return Result.Failure("Email already exists.");
            }

            var phoneExists = await _context.Customers
                .AnyAsync(
                    x => x.PhoneNumber == request.PhoneNumber &&
                         x.Id != request.CustomerId,
                    cancellationToken);

            if (phoneExists)
            {
                return Result.Failure("Phone number already exists.");
            }

            customer.Update(
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber,
                request.PhotoUrl);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
