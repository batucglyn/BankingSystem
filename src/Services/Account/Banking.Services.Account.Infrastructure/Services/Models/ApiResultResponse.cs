using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Infrastructure.Services.Models
{

    public sealed class ApiResultResponse<T>
    {
        public T? Data { get; set; }

        public bool IsSuccess { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
