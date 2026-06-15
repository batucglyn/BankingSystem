using Banking.Services.Account.Domain.Common.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Domain.Events
{
    public sealed record AccountCreatedEvent(
     Guid AccountId,
     Guid CustomerId,
     string IBAN)
     : DomainEvent;
}
