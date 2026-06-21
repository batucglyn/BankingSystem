using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Bus.Events
{
    public sealed record CustomerCreatedEvent(
      Guid CustomerId,
      string FirstName,
      string LastName,
      string Email,
      string PhoneNumber);
}
