using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Bus.Events
{
    public sealed record AccountCreatedEvent(Guid AccountId,
    Guid CustomerId,
    string IBAN);
    
    
}
