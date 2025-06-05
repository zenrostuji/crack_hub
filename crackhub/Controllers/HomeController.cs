using crackhub.Models;
using crackhub.Models.Data;
using crackhub.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace crackhub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGameRepository _gameRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;

        public HomeController(
            ILogger<HomeController> logger, 
            IGameRepository gameRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy 8 game mới nhất
            var latestGames = await _gameRepository.GetRecentGamesAsync(8);
            ViewBag.LatestGames = latestGames;

            // Lấy 8 game phổ biến nhất (dựa trên số lượt tải)
            var popularGames = await _gameRepository.GetPopularGamesAsync(8);
            ViewBag.PopularGames = popularGames;

            // Lấy 8 game có đánh giá cao nhất
            var topRatedGames = await _gameRepository.GetTopRatedGamesAsync(8);
            ViewBag.TopRatedGames = topRatedGames;

            // Lấy danh sách các thể loại
            var categories = await _categoryRepository.GetAllAsync();
            var sortedCategories = categories.OrderBy(c => c.CategoryName).ToList();

            return View(sortedCategories);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Premium()
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Premium", "Home") });
            }

            // Get user details to check current premium status
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Check if user already has premium status
            bool isPremium = user.PremiumExpiryDate.HasValue && user.PremiumExpiryDate.Value > DateTime.Now;
            ViewBag.IsPremium = isPremium;
            ViewBag.PremiumExpiryDate = user.PremiumExpiryDate;
            
            // Create pricing plans
            ViewBag.Plans = new List<dynamic>
            {
                new 
                {
                    Name = "1 Tháng",
                    Price = 49000,
                    Duration = 1,
                    Features = new List<string> 
                    { 
                        "Không giới hạn lượt download",
                        "Tốc độ download nhanh hơn",
                        "Thay đổi tên hiển thị" 
                    },
                    Popular = false,
                    Color = "primary"
                },
                new 
                {
                    Name = "6 Tháng",
                    Price = 249000,
                    Duration = 6,
                    Features = new List<string> 
                    { 
                        "Không giới hạn lượt download",
                        "Tốc độ download nhanh hơn",
                        "Thay đổi tên hiển thị",
                        "Ưu tiên hỗ trợ kỹ thuật", 
                        "Không hiển thị quảng cáo"
                    },
                    Popular = true,
                    Color = "success"
                },
                new 
                {
                    Name = "1 Năm",
                    Price = 399000,
                    Duration = 12,
                    Features = new List<string> 
                    { 
                        "Không giới hạn lượt download",
                        "Tốc độ download nhanh hơn",
                        "Thay đổi tên hiển thị",
                        "Ưu tiên hỗ trợ kỹ thuật", 
                        "Không hiển thị quảng cáo",
                        "Truy cập sớm game mới"
                    },
                    Popular = false,
                    Color = "danger"
                }
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpgradePremium(int durationMonths)
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get user
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Calculate new expiry date
            DateTime newExpiryDate;
            
            // If user already has premium, extend from current expiry
            if (user.PremiumExpiryDate.HasValue && user.PremiumExpiryDate.Value > DateTime.Now)
            {
                newExpiryDate = user.PremiumExpiryDate.Value.AddMonths(durationMonths);
            }
            else
            {
                // Otherwise start from now
                newExpiryDate = DateTime.Now.AddMonths(durationMonths);
            }

            // Update user premium status
            user.PremiumExpiryDate = newExpiryDate;
            await _userRepository.UpdateAsync(user);

            TempData["SuccessMessage"] = $"Chúc mừng! Tài khoản của bạn đã được nâng cấp lên Premium đến ngày {newExpiryDate.ToString("dd/MM/yyyy")}";

            return RedirectToAction("Premium");
        }
    }
}
