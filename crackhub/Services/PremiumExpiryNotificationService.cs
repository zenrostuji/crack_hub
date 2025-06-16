using crackhub.Repositories;
using crackhub.Services;

namespace crackhub.Services
{
    public class PremiumExpiryNotificationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PremiumExpiryNotificationService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(2); // Kiểm tra mỗi 2 giờ để đảm bảo không bỏ lỡ

        public PremiumExpiryNotificationService(
            IServiceProvider serviceProvider, 
            ILogger<PremiumExpiryNotificationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckAndNotifyExpiringPremiumUsers();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while checking premium expiry notifications");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }        private async Task CheckAndNotifyExpiringPremiumUsers()
        {
            using var scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            try
            {
                // Tính toán thời gian: từ 24 giờ nữa đến 25 giờ nữa (khoảng 1 ngày)
                var oneDayFromNow = DateTime.Now.AddDays(1);
                var startCheck = oneDayFromNow.AddHours(-1); // 23 giờ nữa
                var endCheck = oneDayFromNow.AddHours(1);    // 25 giờ nữa

                _logger.LogInformation($"Checking for premium expiry between {startCheck:yyyy-MM-dd HH:mm} and {endCheck:yyyy-MM-dd HH:mm}");

                // Tìm tất cả user có premium hết hạn trong khoảng thời gian này
                var expiringUsers = await userRepository.GetUsersWithPremiumExpiringBetweenAsync(startCheck, endCheck);

                _logger.LogInformation($"Found {expiringUsers.Count()} users with premium expiring in ~1 day");

                foreach (var user in expiringUsers)
                {
                    if (!string.IsNullOrEmpty(user.Email) && user.PremiumExpiryDate.HasValue)
                    {
                        try
                        {
                            // Tính số giờ còn lại
                            var hoursLeft = (user.PremiumExpiryDate.Value - DateTime.Now).TotalHours;
                            
                            // Chỉ gửi nếu còn từ 20-28 giờ (khoảng 1 ngày)
                            if (hoursLeft >= 20 && hoursLeft <= 28)
                            {
                                await emailService.SendPremiumExpiryNotificationAsync(
                                    user.Email, 
                                    user.DisplayName, 
                                    user.PremiumExpiryDate.Value);

                                _logger.LogInformation($"Premium expiry notification sent to user {user.Id} ({user.Email}) - {hoursLeft:F1} hours left");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Failed to send premium expiry notification to user {user.Id} ({user.Email})");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking expiring premium users");
            }
        }
    }
}
