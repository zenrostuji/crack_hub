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
        private readonly IPremiumRepository _premiumRepository;
        private readonly IPremiumRegisterRepository _premiumRegisterRepository;
        private readonly IGameScoreRepository _gameScoreRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(
            ILogger<HomeController> logger, 
            IGameRepository gameRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IPremiumRepository premiumRepository,
            IPremiumRegisterRepository premiumRegisterRepository,
            IGameScoreRepository gameScoreRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _gameRepository = gameRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _premiumRepository = premiumRepository;
            _premiumRegisterRepository = premiumRegisterRepository;
            _gameScoreRepository = gameScoreRepository;
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult GameRating()
        {
            return View();
        }

        // Bảng xếp hạng game
        public async Task<IActionResult> GameLeaderboard(string gameName = "CHIẾN TRƯỜNG")
        {
            var topScores = await _gameScoreRepository.GetTopScoresAsync(gameName, 50);
            ViewBag.GameName = gameName;
            ViewBag.CurrentUserId = HttpContext.Session.GetString("UserId");
            return View(topScores);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitScore([FromBody] GameScoreSubmission submission)
        {
            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                if (userId == null)
                {
                    return Json(new { success = false, message = "Bạn cần đăng nhập để lưu điểm số" });
                }

                var gameScore = new GameScore
                {
                    UserId = userId,
                    GameName = submission.GameName,
                    Score = submission.Score,
                    Level = submission.Level,
                    EnemiesKilled = submission.EnemiesKilled,
                    SurvivalTime = submission.SurvivalTime
                };

                var savedScore = await _gameScoreRepository.AddOrUpdateScoreAsync(gameScore);
                var userRank = await _gameScoreRepository.GetUserRankAsync(userId, submission.GameName);

                return Json(new { 
                    success = true, 
                    message = "Điểm số đã được lưu thành công!",
                    rank = userRank,
                    isNewRecord = savedScore.Score == submission.Score
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting game score");
                return Json(new { success = false, message = "Có lỗi xảy ra khi lưu điểm số" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserRank(string gameName = "CHIẾN TRƯỜNG")
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return Json(new { rank = 0, score = 0 });
            }

            var userScore = await _gameScoreRepository.GetUserGameScoreAsync(userId, gameName);
            var rank = userScore != null ? await _gameScoreRepository.GetUserRankAsync(userId, gameName) : 0;

            return Json(new { 
                rank = rank, 
                score = userScore?.Score ?? 0,
                level = userScore?.Level ?? 0,
                enemiesKilled = userScore?.EnemiesKilled ?? 0,
                survivalTime = userScore?.SurvivalTime ?? 0
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetTopScores(string gameName = "CHIẾN TRƯỜNG", int count = 10)
        {
            var topScores = await _gameScoreRepository.GetTopScoresAsync(gameName, count);
            var scoreList = topScores.Select((score, index) => new
            {
                rank = index + 1,
                playerName = score.User?.DisplayName ?? "Unknown",
                score = score.Score,
                level = score.Level,
                enemiesKilled = score.EnemiesKilled,
                survivalTime = score.SurvivalTime,
                date = score.UpdatedAt.ToString("dd/MM/yyyy")
            });

            return Json(scoreList);
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
            
            // Get premium plans from database
            var premiumPlans = await _premiumRepository.GetAllAsync();
            ViewBag.Plans = premiumPlans.OrderBy(p => p.Id).ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpgradePremium(int packageId)
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

            // Get selected premium package
            var selectedPlan = await _premiumRepository.GetByIdAsync(packageId);
            if (selectedPlan == null)
            {
                return BadRequest("Gói Premium không hợp lệ");
            }

            // Redirect to payment page
            return RedirectToAction("Payment", new { packageId = packageId });
        }

        public async Task<IActionResult> Payment(int packageId)
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Payment", "Home", new { packageId }) });
            }

            // Get user
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Get selected premium package
            var selectedPlan = await _premiumRepository.GetByIdAsync(packageId);
            if (selectedPlan == null)
            {
                return RedirectToAction("Premium");
            }

            ViewBag.User = user;
            ViewBag.Plan = selectedPlan;
            ViewBag.IsPremium = user.PremiumExpiryDate.HasValue && user.PremiumExpiryDate.Value > DateTime.Now;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitPayment(int packageId, IFormFile paymentProof, string? userNotes)
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

            // Get premium package
            var premiumPackage = await _premiumRepository.GetByIdAsync(packageId);
            if (premiumPackage == null)
            {
                return BadRequest("Gói Premium không hợp lệ");
            }

            // Validate payment proof file
            if (paymentProof == null || paymentProof.Length == 0)
            {
                TempData["ErrorMessage"] = "Vui lòng tải lên minh chứng thanh toán";
                return RedirectToAction("Payment", new { packageId });
            }

            // Validate file type and size
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(paymentProof.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                TempData["ErrorMessage"] = "Chỉ chấp nhận file hình ảnh (.jpg, .jpeg, .png)";
                return RedirectToAction("Payment", new { packageId });
            }

            if (paymentProof.Length > 5 * 1024 * 1024) // 5MB
            {
                TempData["ErrorMessage"] = "File quá lớn. Vui lòng chọn file nhỏ hơn 5MB";
                return RedirectToAction("Payment", new { packageId });
            }

            try
            {
                // Save payment proof file
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "payment-proofs");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{userId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await paymentProof.CopyToAsync(fileStream);
                }

                // Create premium register
                var premiumRegister = new PremiumRegister
                {
                    UserId = userId,
                    PackageId = packageId,
                    ProofImageUrl = uniqueFileName,
                    UserComment = userNotes,
                    Status = 2, // Đang chờ
                    RegisterDate = DateTime.Now
                };

                await _premiumRegisterRepository.AddAsync(premiumRegister);

                TempData["SuccessMessage"] = "Yêu cầu thanh toán đã được gửi thành công! Chúng tôi sẽ xem xét và phản hồi trong vòng 24 giờ.";
                return RedirectToAction("PaymentSuccess", new { requestId = premiumRegister.Id });
            }
            catch (Exception ex)
            {
                // Log error
                _logger.LogError(ex, "Error submitting payment request for user {UserId}", userId);
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi gửi yêu cầu. Vui lòng thử lại sau.";
                return RedirectToAction("Payment", new { packageId });
            }
        }

        public async Task<IActionResult> PaymentSuccess(int requestId)
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get premium register
            var premiumRegister = await _premiumRegisterRepository.GetByIdAsync(requestId);
            if (premiumRegister == null || premiumRegister.UserId != userId)
            {
                return NotFound();
            }

            return View(premiumRegister);
        }

        public async Task<IActionResult> PaymentHistory()
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get user's payment history
            var premiumRegisters = await _premiumRegisterRepository.GetByUserIdAsync(userId);
            return View(premiumRegisters);
        }
    }

    // DTO class for score submission
    public class GameScoreSubmission
    {
        public string GameName { get; set; } = "CHIẾN TRƯỜNG";
        public int Score { get; set; }
        public int Level { get; set; }
        public int EnemiesKilled { get; set; }
        public int SurvivalTime { get; set; }
    }
}
