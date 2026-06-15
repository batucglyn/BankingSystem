using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Domain.Common.DomainEvents
{
    public abstract record DomainEvent : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; init; }
            = DateTime.UtcNow;
    }
}
