using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.GetCustomerById
{
    public sealed record GetCustomerByIdQuery(Guid Id) : IRequest<Result<GetCustomerByIdResponse>>;

}
