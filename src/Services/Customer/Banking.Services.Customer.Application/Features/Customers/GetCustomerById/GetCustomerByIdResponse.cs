using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.GetCustomerById;

public sealed record GetCustomerByIdResponse
(
       Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        string IdentityNumber,
        string? PhotoUrl,
        bool IsActive

 );


