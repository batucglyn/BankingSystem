using Banking.Services.Account.Application.Abstractions;
using Banking.Services.Account.Application.Common.Helpers;
using Banking.Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.CreateAccount
{
    public sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result<CreateAccountResponse>>
    {

        private readonly IAccountDbContext _context;

        public CreateAccountCommandHandler(IAccountDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CreateAccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {

            // TODO:
            // CustomerService üzerinden müşteri kontrolü yapılacak


            var accountNumber = AccountNumberGenerator.Generate();
            var iban = IBANGenerator.Generate(accountNumber);


            var account = new Domain.Entities.Account
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                AccountNumber = accountNumber,
                IBAN = iban,
                Currency = request.Currency,
                Balance = 0m,
                Status = Domain.Enums.AccountStatus.Active,
                CreatedAt = DateTime.UtcNow
            };


            await _context.Accounts.AddAsync(account, cancellationToken);

            await _context.SaveChangesAsync(
          cancellationToken);


            return Result<CreateAccountResponse>.Success(
                 new CreateAccountResponse(account.Id, account.AccountNumber, account.IBAN)
             );

          


        }
    }
}
