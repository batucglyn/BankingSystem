using Banking.Services.Customer.Application.Abstractions;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result<GetCustomerByIdResponse>>
    {

        private readonly ICustomerDbContext _context;

        public GetCustomerByIdQueryHandler(ICustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetCustomerByIdResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {


            var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);

            if (customer == null)
            {
                return Result<GetCustomerByIdResponse>.Failure($"Customer not found.{request.Id}");

            }


            var response = new GetCustomerByIdResponse(
                customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.PhoneNumber,
                customer.IdentityNumber,
                customer.PhotoUrl,
                customer.IsActive
            );

            return Result<GetCustomerByIdResponse>.Success(response);

        }
    }
}
