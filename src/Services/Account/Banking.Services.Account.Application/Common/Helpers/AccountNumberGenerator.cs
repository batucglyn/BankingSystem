using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Common.Helpers
{
    public static class AccountNumberGenerator
    {


        public static string Generate()
        {
            return Random.Shared.Next(100000000, 999999999).ToString();
        }
    }
}
