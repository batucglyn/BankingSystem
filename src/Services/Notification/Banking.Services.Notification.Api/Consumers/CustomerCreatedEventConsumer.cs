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

        public async Task Consume(ConsumeContext<CustomerCreatedEvent> context)
        {
            _logger.LogInformation(
                "Customer created event consumed. CustomerId: {CustomerId}, Email: {Email}",
                context.Message.CustomerId,
                context.Message.Email);

            await Task.CompletedTask;
        }
    }
}
