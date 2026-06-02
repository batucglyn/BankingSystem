using Banking.Services.Account.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Domain.ValueObjects
{
    public sealed class Money
    {
        public decimal Amount { get; }

        public CurrencyType Currency { get; }

        public Money(
            decimal amount,
            CurrencyType currency)
        {
            if (amount < 0)
            {
                throw new ArgumentException(
                    "Amount cannot be negative.");
            }

            Amount = amount;
            Currency = currency;
        }

        public Money Add(decimal amount)
        {
            return new Money(
                Amount + amount,
                Currency);
        }

        public Money Subtract(decimal amount)
        {
            if (Amount - amount < 0)
            {
                throw new InvalidOperationException(
                    "Insufficient balance.");
            }

            return new Money(
                Amount - amount,
                Currency);
        }
    }
}
