using Banking.Services.Account.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Domain.Entities
{
    public sealed class AccountTransaction
    {
        public Guid Id { get; private set; }

        public Guid AccountId { get; private set; }

        public decimal Amount { get; private set; }
        public CurrencyType Currency { get; private set; }

        public TransactionType Type { get; private set; }

        public string Description { get; private set; } = default!;

        public DateTime CreatedAt { get; private set; }

        private AccountTransaction()
        {
        }

        public AccountTransaction(
      Guid accountId,
      decimal amount,
      CurrencyType currency,
      TransactionType type,
      string description)
        {
            Id = Guid.NewGuid();

            AccountId = accountId;

            Amount = amount;

            Currency = currency;

            Type = type;

            Description = description;

            CreatedAt = DateTime.UtcNow;

        }
    } }
