using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.ActivateCustomer
{
    public sealed record ActivateCustomerCommand(
     Guid CustomerId)
     : IRequest<Result>;
}
