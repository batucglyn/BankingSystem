using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.CreateCustomer
{
    public sealed record CreateCustomerCommand(
       string FirstName,
       string LastName,
       string Email,
       string PhoneNumber,
       string IdentityNumber,
          string Password)
       : IRequest<Result<CreateCustomerResponse>>;

}
