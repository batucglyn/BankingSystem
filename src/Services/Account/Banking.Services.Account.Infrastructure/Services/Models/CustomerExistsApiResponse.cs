using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Infrastructure.Services.Models
{
    public sealed class CustomerExistsApiResponse
    {
        public bool IsSuccess { get; set; }
        public CustomerExistsData? Data { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public sealed class CustomerExistsData
    {
        public bool Exists { get; set; }
        public bool IsActive { get; set; }
    }
}
