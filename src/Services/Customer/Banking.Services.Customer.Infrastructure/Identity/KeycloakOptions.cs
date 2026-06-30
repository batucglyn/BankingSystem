using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Infrastructure.Identity
{
    public sealed class KeycloakOptions
    {
        public const string SectionName = "Keycloak";

        public string BaseUrl { get; set; } = string.Empty;

        public string Realm { get; set; } = string.Empty;

        public string AdminClientId { get; set; } = string.Empty;

        public string AdminClientSecret { get; set; } = string.Empty;

        public string AdminUserName { get; set; } = string.Empty;

        public string AdminPassword { get; set; } = string.Empty;
    }
}
