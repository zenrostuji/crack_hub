using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace crackhub.Services
{
    public class MailKitEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MailKitEmailService> _logger;

        public MailKitEmailService(IConfiguration configuration, ILogger<MailKitEmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true)
        {
            try
            {
                _logger.LogInformation($"[MailKit] Starting to send email to: {toEmail}");

                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var fromEmail = smtpSettings["FromEmail"] ?? throw new InvalidOperationException("FromEmail not configured");
                var fromName = smtpSettings["FromName"] ?? "CrackHub";
                var smtpServer = smtpSettings["SmtpServer"] ?? throw new InvalidOperationException("SmtpServer not configured");
                var smtpPort = int.Parse(smtpSettings["SmtpPort"] ?? "587");
                var smtpUsername = smtpSettings["Username"] ?? throw new InvalidOperationException("Username not configured");
                var smtpPassword = smtpSettings["Password"] ?? throw new InvalidOperationException("Password not configured");

                _logger.LogInformation($"[MailKit] SMTP Config - Server: {smtpServer}, Port: {smtpPort}, From: {fromEmail}");

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                if (isHtml)
                {
                    bodyBuilder.HtmlBody = body;
                }
                else
                {
                    bodyBuilder.TextBody = body;
                }
                message.Body = bodyBuilder.ToMessageBody();                using var client = new SmtpClient();
                
                try
                {
                    _logger.LogInformation($"[MailKit] Connecting to SMTP server...");
                    
                    // Connect với StartTLS
                    await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                    _logger.LogInformation($"[MailKit] Connected successfully to {smtpServer}:{smtpPort}");
                    
                    // Authenticate
                    _logger.LogInformation($"[MailKit] Authenticating with username: {smtpUsername}");
                    await client.AuthenticateAsync(smtpUsername, smtpPassword);
                    _logger.LogInformation($"[MailKit] Authentication successful");
                    
                    // Send
                    _logger.LogInformation($"[MailKit] Sending message...");
                    await client.SendAsync(message);
                    _logger.LogInformation($"[MailKit] Message sent successfully");
                    
                    // Disconnect
                    await client.DisconnectAsync(true);
                    _logger.LogInformation($"[MailKit] Disconnected from SMTP server");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"[MailKit] SMTP operation failed: {ex.Message}");
                    throw;
                }

                _logger.LogInformation($"[MailKit] Email sent successfully to {toEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[MailKit] Failed to send email to {toEmail}. Error: {ex.Message}");
                throw;
            }
        }

        public async Task SendPremiumExpiryNotificationAsync(string toEmail, string userName, DateTime expiryDate)
        {
            var subject = "⚠️ Thông báo: Premium của bạn sắp hết hạn - CrackHub";
            
            // Tính thời gian còn lại
            var timeLeft = expiryDate - DateTime.Now;
            var hoursLeft = (int)timeLeft.TotalHours;
            var minutesLeft = timeLeft.Minutes;
            
            string timeLeftText;
            if (hoursLeft > 0)
            {
                timeLeftText = $"{hoursLeft} giờ {minutesLeft} phút";
            }
            else
            {
                timeLeftText = $"{minutesLeft} phút";
            }
            
            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #ff6b6b, #ff8e8e); color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .footer {{ padding: 20px; text-align: center; background-color: #333; color: white; border-radius: 0 0 10px 10px; }}
                        .button {{ display: inline-block; padding: 12px 24px; background: linear-gradient(135deg, #4CAF50, #45a049); color: white; text-decoration: none; border-radius: 25px; margin: 10px 0; font-weight: bold; }}
                        .warning {{ color: #ff6b6b; font-weight: bold; }}
                        .time-left {{ background-color: #fff3cd; border: 2px solid #ffeaa7; padding: 15px; border-radius: 8px; text-align: center; margin: 15px 0; }}
                        .benefits {{ background-color: #e8f5e8; padding: 15px; border-radius: 8px; margin: 15px 0; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>🎮 CrackHub Premium</h1>
                            <h2>⚠️ THÔNG BÁO QUAN TRỌNG</h2>
                        </div>
                        <div class='content'>
                            <h2>Xin chào {userName}! 👋</h2>
                            <p>Chúng tôi muốn thông báo rằng <span class='warning'>gói Premium của bạn sắp hết hạn</span>.</p>
                            
                            <div class='time-left'>
                                <h3>⏰ Thời gian còn lại</h3>
                                <h2 style='color: #ff6b6b; margin: 10px 0;'>{timeLeftText}</h2>
                                <p><strong>Hết hạn lúc:</strong> {expiryDate:dd/MM/yyyy HH:mm}</p>
                            </div>
                            
                            <div class='benefits'>
                                <h3>🎯 Tính năng Premium bạn sẽ mất:</h3>
                                <ul>
                                    <li>🚀 Tải game không giới hạn tốc độ</li>
                                    <li>⭐ Truy cập sớm các game mới nhất</li>
                                    <li>🛡️ Hỗ trợ ưu tiên 24/7</li>
                                    <li>🚫 Trải nghiệm không quảng cáo</li>
                                    <li>💎 Tính năng độc quyền cho Premium</li>
                                </ul>
                            </div>
                            
                            <p style='text-align: center; font-size: 18px; color: #ff6b6b;'>
                                <strong>⚡ Hãy gia hạn ngay để không bị gián đoạn dịch vụ!</strong>
                            </p>
                            
                            <p style='text-align: center;'>
                                <a href='#' class='button'>🔄 Gia hạn Premium ngay</a>
                            </p>
                        </div>
                        <div class='footer'>
                            <p>© 2025 CrackHub. Mọi quyền được bảo lưu.</p>
                            <p>📧 Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi.</p>
                        </div>
                    </div>
                </body>
                </html>";

            await SendEmailAsync(toEmail, subject, body, true);
        }

        // Method để test SMTP connection chi tiết
        public async Task TestSmtpConnectionAsync()
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var smtpServer = smtpSettings["SmtpServer"] ?? throw new InvalidOperationException("SmtpServer not configured");
            var smtpPort = int.Parse(smtpSettings["SmtpPort"] ?? "587");
            var smtpUsername = smtpSettings["Username"] ?? throw new InvalidOperationException("Username not configured");
            var smtpPassword = smtpSettings["Password"] ?? throw new InvalidOperationException("Password not configured");

            _logger.LogInformation($"[MailKit] Testing SMTP connection...");
            _logger.LogInformation($"[MailKit] Server: {smtpServer}, Port: {smtpPort}, Username: {smtpUsername}");

            using var client = new SmtpClient();
            
            try
            {
                // Test connection
                _logger.LogInformation($"[MailKit] Attempting to connect...");
                await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                _logger.LogInformation($"[MailKit] ✅ Connection successful");

                // Test authentication
                _logger.LogInformation($"[MailKit] Attempting to authenticate...");
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                _logger.LogInformation($"[MailKit] ✅ Authentication successful");

                // Disconnect
                await client.DisconnectAsync(true);
                _logger.LogInformation($"[MailKit] ✅ Test completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[MailKit] ❌ Test failed: {ex.Message}");
                throw;
            }
        }
    }
}
