using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Common.Helpers
{
    public static class IBANGenerator
    {
        public static string Generate(string accountNumber)
        {
            return $"TR{Random.Shared.Next(10, 99)}000100000{accountNumber}";
        }
    }
}
