using Banking.Services.Account.Domain.Enums;
using Banking.Services.Account.Domain.ValueObjects;
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

        public Money Balance { get; private set; } = default!;

        public AccountStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        private Account()
        {
        }

        public Account(
            Guid customerId,
            string accountNumber,
            string iban,
            CurrencyType currency)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            AccountNumber = accountNumber;
            IBAN = iban;
            Balance = new Money(0, currency);
            Status = AccountStatus.Active;
            CreatedAt = DateTime.UtcNow;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            Balance = Balance.Add(amount);
            UpdatedAt = DateTime.UtcNow;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            if (Balance.Amount < amount)
                throw new InvalidOperationException("Insufficient balance.");

            Balance = Balance.Subtract(amount);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
