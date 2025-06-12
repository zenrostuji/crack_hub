using crackhub.Models.Data;
using crackhub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

namespace crackhub.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IDownloadHistoryRepository _downloadHistoryRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ISearchHistoryRepository _searchHistoryRepository;
        private readonly IFavoriteGameRepository _favoriteGameRepository;
        private readonly IAvatarFrameRepository _avatarFrameRepository;
        private readonly IUserAvatarFrameRepository _userAvatarFrameRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AccountController(
            IUserRepository userRepository,
            IDownloadHistoryRepository downloadHistoryRepository,
            IReviewRepository reviewRepository,
            ISearchHistoryRepository searchHistoryRepository,
            IFavoriteGameRepository favoriteGameRepository,
            IAvatarFrameRepository avatarFrameRepository,
            IUserAvatarFrameRepository userAvatarFrameRepository,
            IWebHostEnvironment hostEnvironment)
        {
            _userRepository = userRepository;
            _downloadHistoryRepository = downloadHistoryRepository;
            _reviewRepository = reviewRepository;
            _searchHistoryRepository = searchHistoryRepository;
            _favoriteGameRepository = favoriteGameRepository;
            _avatarFrameRepository = avatarFrameRepository;
            _userAvatarFrameRepository = userAvatarFrameRepository;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // If user is already logged in, redirect to home
            if (HttpContext.Session.GetString("UserId") != null)
            {
                // If return URL is specified, redirect there instead
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            
            // Store return URL in ViewBag to use in the form post action
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin đăng nhập";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            // Hash the password
            string hashedPassword = HashPassword(password);
            // Find user by username and password using repository
            var user = await _userRepository.AuthenticateAsync(username, hashedPassword);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            // Store user info in session
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", user.DisplayName ?? user.FirstName + " " + user.LastName);
            HttpContext.Session.SetString("UserRole", user.RoleId.ToString());
            // Store avatar URL in session
            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                HttpContext.Session.SetString("AvatarUrl", user.AvatarUrl);
            }
            // Kiểm tra và thêm thông báo Premium dạng popup (chỉ hiện khi còn 3 ngày hoặc ít hơn)
            if (user.PremiumExpiryDate.HasValue)
            {
                var daysRemaining = (user.PremiumExpiryDate.Value.Date - DateTime.Now.Date).Days;
                if (user.PremiumExpiryDate.Value > DateTime.Now)
                {
                    // Chỉ hiển thị popup khi còn 3 ngày hoặc ít hơn
                    if (daysRemaining <= 3)
                    {
                        string popupTitle = "";
                        string popupMessage = "";
                        string popupType = "";

                        if (daysRemaining == 0)
                        {
                            popupTitle = "⚠️ Cảnh báo Premium";
                            popupMessage = "Tài khoản Premium của bạn sẽ hết hạn hôm nay!<br><br>Vui lòng gia hạn ngay để tiếp tục sử dụng các tính năng Premium.";
                            popupType = "danger";
                        }
                        else if (daysRemaining == 1)
                        {
                            popupTitle = "⚠️ Cảnh báo Premium";
                            popupMessage = "Tài khoản Premium của bạn sẽ hết hạn vào ngày mai!<br><br>Vui lòng gia hạn để tiếp tục sử dụng các tính năng Premium.";
                            popupType = "warning";
                        }
                        else
                        {
                            popupTitle = "📅 Thông báo Premium";
                            popupMessage = $"Tài khoản Premium của bạn sẽ hết hạn trong <strong>{daysRemaining} ngày</strong><br>({user.PremiumExpiryDate.Value.ToString("dd/MM/yyyy")})<br><br>Vui lòng gia hạn để tiếp tục sử dụng.";
                            popupType = "warning";
                        }

                        TempData["ShowPremiumPopup"] = "true";
                        TempData["PremiumPopupTitle"] = popupTitle;
                        TempData["PremiumPopupMessage"] = popupMessage;
                        TempData["PremiumPopupType"] = popupType;
                    }
                }
                else
                {
                    // Premium đã hết hạn
                    TempData["ShowPremiumPopup"] = "true";
                    TempData["PremiumPopupTitle"] = "❌ Premium Đã Hết Hạn";
                    TempData["PremiumPopupMessage"] = "Tài khoản Premium của bạn đã hết hạn.<br><br>Vui lòng gia hạn ngay để tiếp tục sử dụng các tính năng Premium.";
                    TempData["PremiumPopupType"] = "danger";
                }
            }
            // If return URL is specified and is local, redirect there
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            // Default redirect to Home
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            // If user is already logged in, redirect to home
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string lastName, string firstName, string displayName, string email, string password, string confirmPassword)
        {
            // Validate input
            if (string.IsNullOrEmpty(lastName) ||string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(displayName) ||string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin đăng ký";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Mật khẩu xác nhận không khớp";
                return View();
            }

            // Check if username already exists
            if (await _userRepository.GetByDisplayNameAsync(displayName) != null)
            {
                ViewBag.ErrorMessage = "Tên đăng nhập đã được sử dụng";
                return View();
            }

            // Check if email already exists
            if (await _userRepository.EmailExistsAsync(email))
            {
                ViewBag.ErrorMessage = "Email đã được sử dụng";
                return View();
            }

            // Create new user
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                LastName = lastName, 
                FirstName = firstName,
                DisplayName = displayName,
                Email = email,
                PasswordHash = HashPassword(password),
                RoleId = 1,
                PremiumExpiryDate = null,
                CreatedAt = DateTime.Now,
            };

            // Handle avatar if provided (code would need to be added here)
            // If avatar URL is set, store it in session
            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                HttpContext.Session.SetString("AvatarUrl", user.AvatarUrl);
            }

            await _userRepository.CreateAsync(user);

            // Store user info in session
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", user.DisplayName);
            HttpContext.Session.SetString("UserRole", user.RoleId.ToString());

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult GoogleLogin(string? returnUrl = null)
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse(string? returnUrl = null)
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            
            if (!result.Succeeded)
            {
                ViewBag.ErrorMessage = "Đăng nhập Google không thành công";
                ViewBag.ReturnUrl = returnUrl;
                return View("Login");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var givenName = claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            var surname = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
            var googleId = claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                ViewBag.ErrorMessage = "Không thể lấy thông tin email từ Google";
                ViewBag.ReturnUrl = returnUrl;
                return View("Login");
            }

            // Check if user already exists with this email
            var existingUser = await _userRepository.GetByEmailAsync(email);
            
            if (existingUser != null)
            {
                // User exists, log them in
                HttpContext.Session.SetString("UserId", existingUser.Id.ToString());
                HttpContext.Session.SetString("UserName", existingUser.DisplayName ?? existingUser.FirstName + " " + existingUser.LastName);
                HttpContext.Session.SetString("UserRole", existingUser.RoleId.ToString());
                
                if (!string.IsNullOrEmpty(existingUser.AvatarUrl))
                {
                    HttpContext.Session.SetString("AvatarUrl", existingUser.AvatarUrl);
                }
            }
            else
            {
                // Create new user
                var newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = givenName ?? "",
                    LastName = surname ?? "",
                    DisplayName = name ?? email.Split('@')[0],
                    Email = email,
                    PasswordHash = HashPassword(Guid.NewGuid().ToString()), // Random password for Google users
                    RoleId = 1, // Regular user
                    PremiumExpiryDate = null,
                    CreatedAt = DateTime.Now,
                    GoogleId = googleId
                };

                await _userRepository.CreateAsync(newUser);

                // Log the new user in
                HttpContext.Session.SetString("UserId", newUser.Id.ToString());
                HttpContext.Session.SetString("UserName", newUser.DisplayName);
                HttpContext.Session.SetString("UserRole", newUser.RoleId.ToString());
            }

            // If return URL is specified and is local, redirect there
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            // Get user details with avatar frames
            var user = await _userRepository.GetByIdAsync(userId);
                
            if (user == null)
            {
                return NotFound();
            }

            // Get all available avatar frames
            var allFrames = await _avatarFrameRepository.GetAllAsync();
            ViewBag.AllAvatarFrames = allFrames;
            
            // Get user's equipped frame
            var equippedFrame = await _userAvatarFrameRepository.GetActiveFrameByUserAsync(userId);
            ViewBag.EquippedFrame = equippedFrame?.AvatarFrame;

            await LoadUserActivityHistory(user.Id);
            
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(string id, string displayName, string email, 
            string currentPassword, string newPassword, string confirmPassword, IFormFile AvatarFile)
        {
            // Get user details
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Check if user is trying to change display name
            if (!string.IsNullOrEmpty(displayName) && user.DisplayName != displayName)
            {
                // Check if user has permission to change DisplayName
                bool canChangeDisplayName = 
                    user.RoleId == 3 || // Admin
                    user.RoleId == 2 || // Moderator
                    (user.PremiumExpiryDate.HasValue && user.PremiumExpiryDate.Value > DateTime.Now); // Premium user
                
                if (!canChangeDisplayName)
                {
                    ModelState.AddModelError("DisplayName", "Chỉ tài khoản Admin, Moderator hoặc Premium mới được thay đổi tên hiển thị");
                    // Load user activity history for the profile view
                    await LoadUserActivityHistory(user.Id);
                    return View("Profile", user);
                }
                
                // Check if the new display name already exists
                if (await _userRepository.GetByDisplayNameAsync(displayName) != null)
                {
                    ModelState.AddModelError("DisplayName", "Tên hiển thị này đã tồn tại");
                    // Load user activity history for the profile view
                    await LoadUserActivityHistory(user.Id);
                    return View("Profile", user);
                }
                
                // Update the display name
                user.DisplayName = displayName;
                HttpContext.Session.SetString("UserName", displayName);
            }

            // Update email if provided
            if (!string.IsNullOrEmpty(email))
            {
                user.Email = email;
            }

            // Handle avatar upload if file is provided
            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                // Create directory if it doesn't exist
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "avartars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Sử dụng DisplayName làm tên file avatar, đảm bảo không có ký tự không hợp lệ
                string fileName = user.DisplayName.Trim();
                // Loại bỏ các ký tự không hợp lệ cho tên file
                fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
                // Thêm phần mở rộng .jpg
                string uniqueFileName = $"{fileName}.jpg";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Nếu file đã tồn tại, xóa để ghi đè
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await AvatarFile.CopyToAsync(fileStream);
                }

                // Update user avatar URL
                user.AvatarUrl = $"/img/avartars/{uniqueFileName}";
                // Update avatar URL in session
                HttpContext.Session.SetString("AvatarUrl", user.AvatarUrl);
            }

            // If user is trying to change password
            if (!string.IsNullOrEmpty(newPassword))
            {
                // Verify current password
                if (string.IsNullOrEmpty(currentPassword) || user.PasswordHash != HashPassword(currentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không chính xác");
                    await LoadUserActivityHistory(user.Id);
                    return View("Profile", user);
                }

                // Verify new password matches confirmation
                if (newPassword != confirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Mật khẩu xác nhận không khớp");
                    await LoadUserActivityHistory(user.Id);
                    return View("Profile", user);
                }

                // Update password
                user.PasswordHash = HashPassword(newPassword);
            }

            await _userRepository.UpdateAsync(user);

            ViewBag.SuccessMessage = "Thông tin tài khoản đã được cập nhật thành công";
            
            // Load user activity history for the profile view
            await LoadUserActivityHistory(user.Id);
            
            return View("Profile", user);
        }

        private async Task LoadUserActivityHistory(string userId)
        {
            try
            {
                // Load recent downloads - get user's download history
                var recentDownloads = await _downloadHistoryRepository.GetDownloadsByUserAsync(userId);
                ViewBag.RecentDownloads = recentDownloads
                    .OrderByDescending(d => d.DownloadDate)
                    .Take(10)
                    .Select(d => new
                    {
                        d.Id,
                        d.GameId,
                        d.DownloadDate,
                        Game = d.Game
                    })
                    .ToList();

                // Load user reviews
                var userReviews = await _reviewRepository.GetReviewsByUserAsync(userId);
                ViewBag.UserReviews = userReviews
                    .OrderByDescending(r => r.DatePosted)
                    .Take(10)
                    .ToList();

                // Load search history
                var searchHistory = await _searchHistoryRepository.GetSearchesByUserAsync(userId);
                ViewBag.SearchHistory = searchHistory
                    .OrderByDescending(s => s.SearchDate)
                    .Take(10)
                    .ToList();
            }
            catch (Exception)
            {
                // Ghi log lỗi và khởi tạo các ViewBag rỗng để tránh lỗi null
                ViewBag.RecentDownloads = new List<object>();
                ViewBag.UserReviews = new List<object>();
                ViewBag.SearchHistory = new List<object>();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            // Get user's favorite games
            var favorites = await _favoriteGameRepository.GetFavoritesByUserAsync(userId);

            return View(favorites);
        }

        [HttpGet]
        public IActionResult GetAvatarFrames()
        {
            try
            {
                // Get all avatar frames from the directory
                string frameDirectory = Path.Combine(_hostEnvironment.WebRootPath, "img", "frameAvartar");
                
                if (!Directory.Exists(frameDirectory))
                {
                    Directory.CreateDirectory(frameDirectory);
                }
                
                var framePaths = Directory.GetFiles(frameDirectory, "*.png")
                                         .Concat(Directory.GetFiles(frameDirectory, "*.jpg"))
                                         .Concat(Directory.GetFiles(frameDirectory, "*.gif"));
                
                var frames = framePaths.Select(path => new
                {
                    name = Path.GetFileNameWithoutExtension(path),
                    path = $"/img/frameAvartar/{Path.GetFileName(path)}"
                }).ToList();
                
                return Json(frames);
            }
            catch (Exception)
            {
                return Json(new List<object>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> EquipAvatarFrame(int frameId)
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thực hiện chức năng này" });
            }

            try
            {
                // Check if the user has this frame
                var userFrame = await _userAvatarFrameRepository.GetByIdAsync(userId, frameId);
                
                if (userFrame != null)
                {
                    // User has this frame, use repository method to set it as active
                    var result = await _userAvatarFrameRepository.SetActiveFrameAsync(userId, frameId);
                    if (!result)
                    {
                        return Json(new { success = false, message = "Không thể trang bị khung avatar" });
                    }
                }
                else
                {
                    // User doesn't have this frame, check if it's a free frame
                    var frame = await _avatarFrameRepository.GetByIdAsync(frameId);
                    if (frame == null)
                    {
                        return Json(new { success = false, message = "Khung avatar không tồn tại" });
                    }
                    
                    if (!frame.IsDefault && !frame.IsPremium)
                    {
                        // This is not a default or premium frame, check if user meets level requirement
                        // TODO: Add user level check logic here if needed
                    }
                    
                    // Add frame to user's collection and equip it
                    userFrame = new UserAvatarFrame
                    {
                        UserId = userId,
                        FrameId = frameId,
                        IsEquipped = true,
                        ObtainedDate = DateTime.Now
                    };
                    await _userAvatarFrameRepository.CreateAsync(userFrame);
                    
                    // Deactivate other frames
                    await _userAvatarFrameRepository.SetActiveFrameAsync(userId, frameId);
                }
                
                var equippedFrame = await _avatarFrameRepository.GetByIdAsync(frameId);
                return Json(new { 
                    success = true, 
                    message = "Đã trang bị khung avatar thành công", 
                    frameUrl = equippedFrame?.FrameUrl 
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> UnequipAvatarFrame()
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thực hiện chức năng này" });
            }

            try
            {
                // Get all equipped frames for the user and unequip them
                var userFrames = await _userAvatarFrameRepository.GetUserAvatarFramesByUserAsync(userId);
                
                foreach (var frame in userFrames.Where(f => f.IsEquipped))
                {
                    frame.IsEquipped = false;
                    await _userAvatarFrameRepository.UpdateAsync(frame);
                }
                
                return Json(new { 
                    success = true, 
                    message = "Đã gỡ khung avatar thành công"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}