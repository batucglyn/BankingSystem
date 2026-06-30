using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Abstractions
{
    public interface ICustomerServiceClient
    {
        Task<bool> CustomerExistsAndActiveAsync(
            Guid customerId,
            CancellationToken cancellationToken = default);





        Task<CustomerByKeycloakUserIdResponse?> GetCustomerByKeycloakUserIdAsync(
    string keycloakUserId,
    CancellationToken cancellationToken);
    }
}
