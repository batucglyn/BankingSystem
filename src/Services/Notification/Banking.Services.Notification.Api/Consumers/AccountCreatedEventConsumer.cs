using Banking.Bus.Events;
using MassTransit;

namespace Banking.Services.Notification.Api.Consumers
{
    public sealed class AccountCreatedEventConsumer
    : IConsumer<AccountCreatedEvent>
    {
        private const string CorrelationIdHeaderName = "X-Correlation-Id";

        private readonly ILogger<AccountCreatedEventConsumer> _logger;

        public AccountCreatedEventConsumer(
            ILogger<AccountCreatedEventConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AccountCreatedEvent> context)
        {

            _logger.LogInformation(
                "Account created event consumed. AccountId: {AccountId}, CustomerId: {CustomerId}, IBAN: {IBAN}",
                context.Message.AccountId,
                context.Message.CustomerId,
                context.Message.IBAN);


            await Task.CompletedTask;
        }
    }
}
