using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.CheckCustomerExists
{
    public sealed record CheckCustomerExistsQuery(
        Guid CustomerId)
        : IRequest<Result<CheckCustomerExistsResponse>>;
}
