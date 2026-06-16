using Banking.Bus.Events;
using MassTransit;

namespace Banking.Services.Notification.Api.Consumers
{
    public sealed class AccountCreatedEventConsumer
      : IConsumer<AccountCreatedEvent>
    {
        private readonly ILogger<AccountCreatedEventConsumer> _logger;

        public AccountCreatedEventConsumer(
            ILogger<AccountCreatedEventConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(
            ConsumeContext<AccountCreatedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation(
                "Account created notification received. AccountId: {AccountId}, CustomerId: {CustomerId}, IBAN: {IBAN}",
                message.AccountId,
                message.CustomerId,
                message.IBAN);

            await Task.CompletedTask;
        }
    }
}
