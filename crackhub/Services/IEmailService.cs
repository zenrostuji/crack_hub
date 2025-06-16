namespace crackhub.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true);
        Task SendPremiumExpiryNotificationAsync(string toEmail, string userName, DateTime expiryDate);
    }
}
