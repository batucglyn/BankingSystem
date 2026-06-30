using Banking.Services.Account.Application.Abstractions;
using Banking.Services.Account.Infrastructure.Services.Models;
using Banking.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Banking.Services.Account.Infrastructure.Services
{
    public sealed class CustomerServiceClient : ICustomerServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public CustomerServiceClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CustomerExistsAndActiveAsync(
      Guid customerId,
      CancellationToken cancellationToken = default)
        {
            try
            {
                AddAuthorizationHeaderIfExists();
                var response = await _httpClient.GetFromJsonAsync<CustomerExistsApiResponse>(
                    $"api/customers/{customerId}/exists",
                    JsonOptions,
                    cancellationToken);

                return response is not null
                       && response.IsSuccess
                       && response.Data is not null
                       && response.Data.Exists
                       && response.Data.IsActive;
            }
            catch
            {
                throw new CustomerServiceUnavailableException();
            }
        }
        public async Task<CustomerByKeycloakUserIdResponse?> GetCustomerByKeycloakUserIdAsync(
      string keycloakUserId,
      CancellationToken cancellationToken)
        {
            AddAuthorizationHeaderIfExists();

            var response = await _httpClient.GetAsync(
                $"/api/customers/by-keycloak-user/{keycloakUserId}",
                cancellationToken);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content
                .ReadFromJsonAsync<ApiResultResponse<CustomerByKeycloakUserIdResponse>>(
                    cancellationToken: cancellationToken);

            return result?.Data;
        }

        private void AddAuthorizationHeaderIfExists()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext?
                .Request
                .Headers
                .Authorization
                .ToString();

            if (string.IsNullOrWhiteSpace(authorizationHeader))
                return;

            if (!AuthenticationHeaderValue.TryParse(
                authorizationHeader,
                out var headerValue))
            {
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = headerValue;
        }
    }
}

