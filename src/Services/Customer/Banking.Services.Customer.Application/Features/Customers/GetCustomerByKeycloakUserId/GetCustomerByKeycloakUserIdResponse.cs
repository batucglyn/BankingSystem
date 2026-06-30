using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.GetCustomerByKeycloakUserId
{
    public sealed record GetCustomerByKeycloakUserIdResponse(
    Guid CustomerId,
    string KeycloakUserId,
    bool IsActive);
}
