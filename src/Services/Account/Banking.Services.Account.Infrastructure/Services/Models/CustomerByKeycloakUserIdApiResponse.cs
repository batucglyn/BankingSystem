using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Infrastructure.Services.Models
{
    public sealed record CustomerByKeycloakUserIdApiResponse(
       Guid CustomerId,
       string KeycloakUserId,
       bool IsActive);
}
