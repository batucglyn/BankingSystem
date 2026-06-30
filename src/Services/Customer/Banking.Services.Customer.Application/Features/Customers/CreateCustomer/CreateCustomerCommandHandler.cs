using Banking.Bus.Events;
using Banking.Outbox;
using Banking.Services.Customer.Application.Abstractions;
using Banking.Services.Customer.Application.Abstractions.Identity;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Banking.Services.Customer.Application.Features.Customers.CreateCustomer;

public class CreateCustomerCommandHandler
    : IRequestHandler<CreateCustomerCommand, Result<CreateCustomerResponse>>
{
    private readonly ILogger<CreateCustomerCommandHandler> _logger;
    private readonly ICustomerDbContext _context;
    private readonly IIdentityService _identityService;

    public CreateCustomerCommandHandler(
        ICustomerDbContext context,
        ILogger<CreateCustomerCommandHandler> logger,
        IIdentityService identityService)
    {
        _context = context;
        _logger = logger;
        _identityService = identityService;
    }

    public async Task<Result<CreateCustomerResponse>> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        if (await _context.Customers
            .AnyAsync(x => x.Email == request.Email, cancellationToken))
            return Result<CreateCustomerResponse>.Failure("Email already exists.");

        if (await _context.Customers
            .AnyAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken))
            return Result<CreateCustomerResponse>.Failure("Phone number already exists.");

        if (await _context.Customers
            .AnyAsync(x => x.IdentityNumber == request.IdentityNumber, cancellationToken))
            return Result<CreateCustomerResponse>.Failure("Identity number already exists.");

        var keycloakUserId = await _identityService.CreateUserAsync(
            new CreateIdentityUserRequest(
                Username: request.Email,
                Email: request.Email,
                FirstName: request.FirstName,
                LastName: request.LastName,
                Password: request.Password),
            cancellationToken);

        var customer = new Domain.Entities.Customer(
            keycloakUserId,
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.IdentityNumber);

        await _context.Customers.AddAsync(customer, cancellationToken);

        _logger.LogInformation(
            "Customer created. CustomerId: {CustomerId}, KeycloakUserId: {KeycloakUserId}, Email: {Email}",
            customer.Id,
            customer.KeycloakUserId,
            customer.Email);

        var customerCreatedEvent = new CustomerCreatedEvent(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.PhoneNumber);

        var outboxMessage = new OutboxMessage
        {
            Id = Guid.CreateVersion7(),
            Type = typeof(CustomerCreatedEvent).AssemblyQualifiedName!,
            Content = JsonSerializer.Serialize(customerCreatedEvent),
            CreatedAt = DateTime.UtcNow,
            RetryCount = 0
        };

        await _context.OutboxMessages.AddAsync(outboxMessage, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<CreateCustomerResponse>.Success(
            new CreateCustomerResponse(customer.Id));
    }
}