using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.GetCustomers
{
    public sealed record GetCustomersQuery()
    : IRequest<Result<List<GetCustomersResponse>>>;
}
