using crackhub.Repositories;
using crackhub.Services;
using Microsoft.AspNetCore.Mvc;

namespace crackhub.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<NotificationController> _logger;
        private readonly IConfiguration _configuration;

        public NotificationController(
            IEmailService emailService, 
            IUserRepository userRepository,
            ILogger<NotificationController> logger,
            IConfiguration configuration)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
        }
        // Action để gửi email thông báo premium sắp hết hạn thủ công (cho admin)        [HttpPost]
        public async Task<IActionResult> SendPremiumExpiryNotifications()
        {
            try
            {
                // Tìm user có premium hết hạn trong 3 ngày tới (để test dễ hơn)
                var now = DateTime.Now;
                var threeDaysFromNow = now.AddDays(3);

                _logger.LogInformation($"Checking for users with premium expiring between {now:yyyy-MM-dd HH:mm} and {threeDaysFromNow:yyyy-MM-dd HH:mm}");

                // Tìm tất cả user có premium hết hạn trong 3 ngày tới
                var expiringUsers = await _userRepository.GetUsersWithPremiumExpiringBetweenAsync(now, threeDaysFromNow);

                _logger.LogInformation($"Found {expiringUsers.Count()} users with premium expiring in next 3 days");

                int successCount = 0;
                int failureCount = 0;

                foreach (var user in expiringUsers)
                {
                    _logger.LogInformation($"Processing user: {user.Id}, Email: {user.Email}, Premium Expiry: {user.PremiumExpiryDate}, EmailConfirmed: {user.EmailConfirmed}");

                    if (!string.IsNullOrEmpty(user.Email) && user.PremiumExpiryDate.HasValue)
                    {
                        try
                        {
                            await _emailService.SendPremiumExpiryNotificationAsync(
                                user.Email, 
                                user.DisplayName, 
                                user.PremiumExpiryDate.Value);
                            
                            successCount++;
                            _logger.LogInformation($"SUCCESS: Premium expiry notification sent to user {user.Id} ({user.Email})");
                        }
                        catch (Exception ex)
                        {
                            failureCount++;
                            _logger.LogError(ex, $"FAILED: Could not send premium expiry notification to user {user.Id} ({user.Email}). Error: {ex.Message}");
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"SKIPPED: User {user.Id} - Email: '{user.Email}', PremiumExpiryDate: {user.PremiumExpiryDate}");
                    }
                }

                var message = $"Đã gửi thành công {successCount} email. Thất bại: {failureCount} email. Tổng user tìm thấy: {expiringUsers.Count()}";
                TempData["Message"] = message;
                _logger.LogInformation(message);
                
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                var errorMsg = $"Error occurred while sending premium expiry notifications manually: {ex.Message}";
                _logger.LogError(ex, errorMsg);
                TempData["Error"] = $"Có lỗi xảy ra: {ex.Message}";
                return RedirectToAction("Index", "Admin");
            }
        }// Action để gửi email test
        [HttpPost]
        public async Task<IActionResult> SendTestEmail(string email, string userName)
        {
            try
            {
                _logger.LogInformation($"Attempting to send test email to: {email}, userName: {userName}");

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(userName))
                {
                    TempData["Error"] = "Vui lòng nhập đầy đủ email và tên người dùng.";
                    return RedirectToAction("Index", "Admin");
                }

                await _emailService.SendPremiumExpiryNotificationAsync(
                    email, 
                    userName, 
                    DateTime.Now.AddDays(1));

                _logger.LogInformation($"SUCCESS: Test email sent to {email}");
                TempData["Message"] = $"Đã gửi email test thành công tới {email}";
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                var errorMsg = $"Failed to send test email to {email}: {ex.Message}";
                _logger.LogError(ex, errorMsg);
                TempData["Error"] = $"Lỗi gửi email: {ex.Message}";
                return RedirectToAction("Index", "Admin");
            }
        }        // Action để tạo user test với premium sắp hết hạn (để test thông báo)
        [HttpPost]
        public async Task<IActionResult> CreateTestPremiumUser()
        {
            try
            {
                var testUser = new crackhub.Models.Data.User
                {
                    Id = "test-premium-" + Guid.NewGuid().ToString("N")[..8],
                    FirstName = "Test",
                    LastName = "Premium User",
                    DisplayName = "TestPremium" + DateTime.Now.ToString("HHmm"),
                    Email = "conansleuth4869@gmail.com", // Dùng email thật để test
                    EmailConfirmed = true,
                    RoleId = 1,
                    PremiumExpiryDate = DateTime.Now.AddHours(48), // Hết hạn sau 48 giờ (2 ngày)
                    CreatedAt = DateTime.Now
                };

                await _userRepository.CreateAsync(testUser);
                
                _logger.LogInformation($"Created test user: ID={testUser.Id}, Email={testUser.Email}, PremiumExpiry={testUser.PremiumExpiryDate}");
                
                TempData["Message"] = $"✅ Đã tạo user test '{testUser.DisplayName}' với Premium hết hạn lúc {testUser.PremiumExpiryDate:dd/MM/yyyy HH:mm}. Email: {testUser.Email}";
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create test premium user");
                TempData["Error"] = $"Lỗi tạo user test: {ex.Message}";
                return RedirectToAction("Index", "Admin");
            }
        }

        // Action để kiểm tra tất cả user có Premium trong database
        [HttpPost]
        public async Task<IActionResult> CheckAllPremiumUsers()
        {
            try
            {
                var allUsers = await _userRepository.GetAllAsync();
                var premiumUsers = allUsers.Where(u => u.PremiumExpiryDate.HasValue).ToList();
                
                _logger.LogInformation($"=== CHECKING ALL PREMIUM USERS ===");
                _logger.LogInformation($"Total users in database: {allUsers.Count()}");
                _logger.LogInformation($"Users with premium: {premiumUsers.Count}");
                  foreach (var user in premiumUsers)
                {
                    if (user.PremiumExpiryDate.HasValue)
                    {
                        var timeLeft = user.PremiumExpiryDate.Value - DateTime.Now;
                        _logger.LogInformation($"User: {user.DisplayName} (ID: {user.Id})");
                        _logger.LogInformation($"  Email: {user.Email}");
                        _logger.LogInformation($"  Premium Expiry: {user.PremiumExpiryDate:yyyy-MM-dd HH:mm}");
                        _logger.LogInformation($"  Time Left: {timeLeft.TotalHours:F1} hours");
                        _logger.LogInformation($"  EmailConfirmed: {user.EmailConfirmed}");
                        _logger.LogInformation($"---");
                    }
                }
                
                var expiringSoon = premiumUsers.Where(u => 
                    u.PremiumExpiryDate.HasValue && 
                    u.PremiumExpiryDate.Value >= DateTime.Now && 
                    u.PremiumExpiryDate.Value <= DateTime.Now.AddDays(3)
                ).ToList();
                
                TempData["Message"] = $"📊 Tổng user: {allUsers.Count()}, Premium: {premiumUsers.Count}, Sắp hết hạn (3 ngày): {expiringSoon.Count}. Xem logs để biết chi tiết.";
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check premium users");
                TempData["Error"] = $"Lỗi kiểm tra premium users: {ex.Message}";
                return RedirectToAction("Index", "Admin");
            }
        }
    }
}
