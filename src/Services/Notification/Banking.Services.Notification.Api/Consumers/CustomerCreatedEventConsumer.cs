using Banking.Bus.Events;
using Banking.Services.Notification.Api.Services;
using MassTransit;

namespace Banking.Services.Notification.Api.Consumers
{
    public sealed class CustomerCreatedEventConsumer
        : IConsumer<CustomerCreatedEvent>
    {
        private readonly ILogger<CustomerCreatedEventConsumer> _logger;
        private readonly IEmailSender _emailSender;

        public CustomerCreatedEventConsumer(
            ILogger<CustomerCreatedEventConsumer> logger,
            IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task Consume(ConsumeContext<CustomerCreatedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation(
                "Customer created event consumed. CustomerId: {CustomerId}, Email: {Email}",
                message.CustomerId,
                message.Email);

            var subject = "Welcome to Banking System";

            var body =
                $"Hello {message.FirstName} {message.LastName},\n\n" +
                "Your customer profile has been created successfully.\n\n" +
                "Welcome to Banking System.";

            await _emailSender.SendAsync(
                message.Email,
                subject,
                body,
                context.CancellationToken);

            _logger.LogInformation(
                "Welcome email sent. CustomerId: {CustomerId}, Email: {Email}",
                message.CustomerId,
                message.Email);
        }
    }
}