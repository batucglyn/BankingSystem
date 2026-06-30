using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Abstractions.Identity
{
    public sealed record CreateIdentityUserRequest(
     string Username,
     string Email,
     string FirstName,
     string LastName,
     string Password);
}
