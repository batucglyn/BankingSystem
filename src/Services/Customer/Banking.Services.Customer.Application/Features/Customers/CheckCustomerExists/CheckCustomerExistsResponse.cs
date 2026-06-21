using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.CheckCustomerExists
{
    public sealed record CheckCustomerExistsResponse(
       bool Exists,
       bool IsActive);
}
