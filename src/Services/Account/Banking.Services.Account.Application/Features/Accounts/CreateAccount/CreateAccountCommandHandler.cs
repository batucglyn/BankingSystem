using Banking.Bus.Events;
using Banking.Services.Account.Application.Abstractions;
using Banking.Services.Account.Application.Common.Helpers;
using Banking.Services.Account.Domain.ValueObjects;
using Banking.Shared.Results;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.CreateAccount
{
    public sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result<CreateAccountResponse>>
    {

        private readonly IAccountDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ICustomerServiceClient _customerServiceClient;


        public CreateAccountCommandHandler(IAccountDbContext context, IPublishEndpoint publishEndpoint, ICustomerServiceClient customerServiceClient)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
            _customerServiceClient = customerServiceClient;
        }

        public async Task<Result<CreateAccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {

            // checking if customer exists and is active by calling the customer service
            var customerExists = await _customerServiceClient
                    .CustomerExistsAndActiveAsync(
                        request.CustomerId,
                        cancellationToken);

                if (!customerExists)
                {
                    return Result<CreateAccountResponse>
                        .Failure("Customer not found or inactive.");
                }
      


            var accountNumber = AccountNumberGenerator.Generate();
            var iban = IBANGenerator.Generate(accountNumber);

            var account = new Domain.Entities.Account(
                request.CustomerId,
                accountNumber,
                iban,
                request.Currency
            );

            await _context.Accounts.AddAsync(account, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            await _publishEndpoint.Publish(new AccountCreatedEvent(account.Id, account.CustomerId, account.IBAN), cancellationToken);


            return Result<CreateAccountResponse>.Success(
                new CreateAccountResponse(
                    account.Id,
                    account.AccountNumber,
                    account.IBAN
                )
            );




        }
    }
}
