using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.GetCustomers
{
    public sealed record GetCustomersResponse(
      Guid Id,
      string FirstName,
      string LastName,
      string Email,
      string PhoneNumber,
      bool IsActive);
}
