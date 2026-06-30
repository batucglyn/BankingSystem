using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Abstractions
{

    public sealed record CustomerByKeycloakUserIdResponse(
        Guid CustomerId,
        string KeycloakUserId,
        bool IsActive);
}
