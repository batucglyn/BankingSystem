using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Banking.Authentication.CurrentUser
{
    public sealed class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public string? UserId =>
            User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User?.FindFirstValue("sub");

        public string? Username =>
            User?.FindFirstValue("preferred_username");

        public string? Email =>
            User?.FindFirstValue(ClaimTypes.Email)
            ?? User?.FindFirstValue("email");

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

        public bool IsInRole(string role) =>
            User?.IsInRole(role) ?? false;
    }
}
