using Banking.Services.Account.Application.Abstractions;
using Banking.Services.Account.Infrastructure.Services.Models;
using Banking.Shared.Exceptions;
using System.Net.Http.Json;
using System.Text.Json;

namespace Banking.Services.Account.Infrastructure.Services
{
    public sealed class CustomerServiceClient : ICustomerServiceClient
    {
        private readonly HttpClient _httpClient;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public CustomerServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CustomerExistsAndActiveAsync(
      Guid customerId,
      CancellationToken cancellationToken = default)
        {
            try
            {
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
    }
}
