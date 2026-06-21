using Banking.Shared.Results;
using MediatR;

namespace Banking.Services.Customer.Api.Endpoints.Customers.UpdateCustomer
{
    public sealed record UpdateCustomerCommand(
     Guid CustomerId,
     string FirstName,
     string LastName,
     string Email,
     string PhoneNumber,
     string? PhotoUrl)
     : IRequest<Result>;
}
