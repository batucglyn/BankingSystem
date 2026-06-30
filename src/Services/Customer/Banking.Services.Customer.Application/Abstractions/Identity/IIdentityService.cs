using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Abstractions.Identity
{
    public interface IIdentityService
    {
        Task<string> CreateUserAsync(CreateIdentityUserRequest request,CancellationToken cancellationToken);

    }
}
