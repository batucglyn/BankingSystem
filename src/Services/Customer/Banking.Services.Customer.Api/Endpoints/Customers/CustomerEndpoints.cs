using Banking.Services.Customer.Api.Endpoints.Customers.CreateCustomer;
using Banking.Services.Customer.Api.Endpoints.Customers.GetCustomer;

namespace Banking.Services.Customer.Api.Endpoints.Customers
{
    public static class CustomerEndpoints
    {
        public static RouteGroupBuilder MapCustomerEndpoints(
         this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/customers")
                .WithTags("Customers");

            group.MapCreateCustomerEndpoint();
            group.MapGetCustomerByIdEndpoint();
            return group;
          
        }
    }
}
