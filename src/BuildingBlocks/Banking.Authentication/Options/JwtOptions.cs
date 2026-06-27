using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Authentication.Options
{

    public sealed class JwtOptions
    {
        public const string SectionName = "Keycloak";

        public string Authority { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;
    }
}
