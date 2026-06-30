using Banking.Services.Notification.Api.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Banking.Services.Notification.Api.Services
{
    public sealed class SmtpEmailSender : IEmailSender
    {
        private readonly EmailOptions _emailOptions;
        private readonly ILogger<SmtpEmailSender> _logger;

        public SmtpEmailSender(
            IOptions<EmailOptions> emailOptions,
            ILogger<SmtpEmailSender> logger)
        {
            _emailOptions = emailOptions.Value;
            _logger = logger;
        }

        public async Task SendAsync(
            string to,
            string subject,
            string body,
            CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                _emailOptions.FromName,
                _emailOptions.From));

            message.To.Add(MailboxAddress.Parse(to));

            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var smtpClient = new SmtpClient();

            var secureSocketOptions = _emailOptions.EnableSsl
                ? SecureSocketOptions.StartTls
                : SecureSocketOptions.None;

            await smtpClient.ConnectAsync(
                _emailOptions.Host,
                _emailOptions.Port,
                secureSocketOptions,
                cancellationToken);

            await smtpClient.AuthenticateAsync(
                _emailOptions.UserName,
                _emailOptions.Password,
                cancellationToken);

            await smtpClient.SendAsync(message, cancellationToken);

            await smtpClient.DisconnectAsync(true, cancellationToken);

            _logger.LogInformation(
                "Email sent successfully. To: {To}, Subject: {Subject}",
                to,
                subject);
        }
    }
}
