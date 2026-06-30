using Banking.Authentication.CurrentUser;
using Banking.Bus.Events;
using Banking.Outbox;
using Banking.Services.Account.Application.Abstractions;
using Banking.Services.Account.Application.Common.Helpers;
using Banking.Shared.Correlation;
using Banking.Shared.Results;
using MediatR;
using System.Text.Json;

namespace Banking.Services.Account.Application.Features.Accounts.CreateAccount
{
    public sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result<CreateAccountResponse>>
    {

        private readonly IAccountDbContext _context;
       
        private readonly ICustomerServiceClient _customerServiceClient;
        private readonly ICurrentUser _currentUser;

        public CreateAccountCommandHandler(IAccountDbContext context, ICustomerServiceClient customerServiceClient, ICurrentUser currentUser)
        {
            _context = context;
            _customerServiceClient = customerServiceClient;
            _currentUser = currentUser;
        }

        public async Task<Result<CreateAccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {

            // checking if customer exists and is active by calling the customer service
            if (!_currentUser.IsAuthenticated ||
     string.IsNullOrWhiteSpace(_currentUser.UserId))
            {
                return Result<CreateAccountResponse>
                    .Failure("User is not authenticated.");
            }

            var customer = await _customerServiceClient
                .GetCustomerByKeycloakUserIdAsync(
                    _currentUser.UserId,
                    cancellationToken);

            if (customer is null)
            {
                return Result<CreateAccountResponse>
                    .Failure("Customer profile not found.");
            }

            if (!customer.IsActive)
            {
                return Result<CreateAccountResponse>
                    .Failure("Customer is not active.");
            }



            var accountNumber = AccountNumberGenerator.Generate();
            var iban = IBANGenerator.Generate(accountNumber);

            var account = new Domain.Entities.Account(
                customer.CustomerId,
                accountNumber,
                iban,
                request.Currency
            );

            await _context.Accounts.AddAsync(account, cancellationToken);

            var accountCreatedEvent = new AccountCreatedEvent(
                account.Id,
                account.CustomerId,
                account.IBAN);

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.CreateVersion7(),
                Type = typeof(AccountCreatedEvent).AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(accountCreatedEvent),
                CreatedAt = DateTime.UtcNow
            };

            await _context.OutboxMessages.AddAsync(
                outboxMessage,
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);




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
