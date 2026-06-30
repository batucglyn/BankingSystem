using Banking.Services.Customer.Application.Abstractions.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Banking.Services.Customer.Infrastructure.Identity
{
    public sealed class KeycloakIdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly KeycloakOptions _options;

        public KeycloakIdentityService(
            HttpClient httpClient,
            IOptions<KeycloakOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<string> CreateUserAsync(
            CreateIdentityUserRequest request,
            CancellationToken cancellationToken)
        {
            var adminAccessToken = await GetAdminAccessTokenAsync(cancellationToken);

            var userId = await CreateKeycloakUserAsync(
                request,
                adminAccessToken,
                cancellationToken);

            return userId;
        }

        private async Task<string> GetAdminAccessTokenAsync(
            CancellationToken cancellationToken)
        {
            var tokenEndpoint =
                $"{_options.BaseUrl}/realms/master/protocol/openid-connect/token";
            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["client_id"] = _options.AdminClientId,
                ["username"] = _options.AdminUserName,
                ["password"] = _options.AdminPassword
            };

            if (!string.IsNullOrWhiteSpace(_options.AdminClientSecret))
            {
                form["client_secret"] = _options.AdminClientSecret;
            }

            using var content = new FormUrlEncodedContent(form);

            using var response = await _httpClient.PostAsync(
                tokenEndpoint,
                content,
                cancellationToken);

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Keycloak admin token could not be obtained. Status: {response.StatusCode}, Response: {responseContent}");
            }

            using var document = JsonDocument.Parse(responseContent);

            var accessToken = document.RootElement
                .GetProperty("access_token")
                .GetString();

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new InvalidOperationException("Keycloak admin access token is missing.");
            }

            return accessToken;
        }

        private async Task<string> CreateKeycloakUserAsync(
            CreateIdentityUserRequest request,
            string adminAccessToken,
            CancellationToken cancellationToken)
        {
            var createUserEndpoint =
                $"{_options.BaseUrl}/admin/realms/{_options.Realm}/users";

            var payload = new
            {
                username = request.Username,
                email = request.Email,
                firstName = request.FirstName,
                lastName = request.LastName,
                enabled = true,
                emailVerified = false,
                credentials = new[]
                {
                new
                {
                    type = "password",
                    value = request.Password,
                    temporary = false
                }
            }
            };

            var json = JsonSerializer.Serialize(payload);

            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                createUserEndpoint);

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", adminAccessToken);

            httpRequest.Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            using var response = await _httpClient.SendAsync(
                httpRequest,
                cancellationToken);

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Keycloak user could not be created. Status: {response.StatusCode}, Response: {responseContent}");
            }

            var location = response.Headers.Location?.ToString();

            if (string.IsNullOrWhiteSpace(location))
            {
                throw new InvalidOperationException("Keycloak user location header is missing.");
            }

            return location.Split('/').Last();
        }
    }
}
