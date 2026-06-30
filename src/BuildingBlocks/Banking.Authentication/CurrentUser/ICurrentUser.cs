using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Authentication.CurrentUser
{
    public interface ICurrentUser
    {
        string? UserId { get; }

        string? Username { get; }

        string? Email { get; }

        bool IsAuthenticated { get; }

        bool IsInRole(string role);
    }
}
