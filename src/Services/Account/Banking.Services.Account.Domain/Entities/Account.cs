using Banking.Services.Account.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Domain.Entities
{
    public class Account
    {

        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        public string AccountNumber { get; set; } = default!;

        public string IBAN { get; set; } = default!;

        public decimal Balance { get; set; }

        public CurrencyType Currency { get; set; }

        public AccountStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }


        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException(
                    "Amount must be greater than zero.");
            }

            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException(
                    "Amount must be greater than zero.");
            }

            if (Balance - amount < 0)
            {
                throw new InvalidOperationException(
                    "Insufficient balance.");
            }

            Balance -= amount;
        }
    }
}
