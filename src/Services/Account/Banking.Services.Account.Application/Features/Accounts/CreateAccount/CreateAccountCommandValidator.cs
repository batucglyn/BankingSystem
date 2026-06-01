using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.CreateAccount
{
    public class CreateAccountCommandValidator:AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
            RuleFor(x => x.Currency).IsInEnum().WithMessage("Invalid currency type.");
        }
    }
}
