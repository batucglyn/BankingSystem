using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Shared.Exceptions
{
    public sealed class CustomerServiceUnavailableException : Exception
    {
        public CustomerServiceUnavailableException()
            : base("Customer service is currently unavailable.")
        {
        }
    }
}
