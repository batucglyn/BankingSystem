using Banking.Bus.Events;
using Banking.Services.Customer.Application.Abstractions;
using Banking.Shared.Results;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Features.Customers.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CreateCustomerResponse>>
{



    private readonly ICustomerDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    public CreateCustomerCommandHandler(ICustomerDbContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Result<CreateCustomerResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
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

        var customer = new Domain.Entities.Customer(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.IdentityNumber);

        await _context.Customers.AddAsync(customer, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);


        await _publishEndpoint.Publish(new CustomerCreatedEvent(

            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.PhoneNumber), cancellationToken);
    

            

        return Result<CreateCustomerResponse>.Success(
            new CreateCustomerResponse(customer.Id));




    }
}

