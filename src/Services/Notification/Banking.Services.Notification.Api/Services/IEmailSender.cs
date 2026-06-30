namespace Banking.Services.Notification.Api.Services
{
    public interface IEmailSender
    {
        Task SendAsync(string to,string subject,string body,CancellationToken cancellationToken = default);
    }
}
