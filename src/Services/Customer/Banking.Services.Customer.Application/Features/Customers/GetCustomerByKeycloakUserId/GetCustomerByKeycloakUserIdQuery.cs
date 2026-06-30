using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.GetCustomerByKeycloakUserId
{
    public sealed record GetCustomerByKeycloakUserIdQuery(
       string KeycloakUserId)
       : IRequest<Result<GetCustomerByKeycloakUserIdResponse>>;
}
