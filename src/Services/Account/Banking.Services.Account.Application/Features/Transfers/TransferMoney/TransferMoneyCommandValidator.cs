using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Transfers.TransferMoney
{
    public sealed class TransferMoneyCommandValidator
    : AbstractValidator<TransferMoneyCommand>
    {
        public TransferMoneyCommandValidator()
        {
            RuleFor(x => x.FromAccountId)
                .NotEmpty();

            RuleFor(x => x.ToAccountId)
                .NotEmpty();

            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x)
                .Must(x => x.FromAccountId != x.ToAccountId)
                .WithMessage(
                    "Transfer accounts cannot be the same.");
        }
    }
}
