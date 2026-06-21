using Banking.Bus.Events;
using MassTransit;

namespace Banking.Services.Notification.Api.Consumers
{
    public sealed class CustomerCreatedEventConsumer
     : IConsumer<CustomerCreatedEvent>
    {
        private readonly ILogger<CustomerCreatedEventConsumer> _logger;

        public CustomerCreatedEventConsumer(
            ILogger<CustomerCreatedEventConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(
            ConsumeContext<CustomerCreatedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation(
                "New customer created. CustomerId: {CustomerId}, Email: {Email}",
                message.CustomerId,
                message.Email);

            await Task.CompletedTask;
        }
    }
}
