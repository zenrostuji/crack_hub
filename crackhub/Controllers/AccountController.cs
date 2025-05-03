using crackhub.Models.Data;
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

namespace crackhub.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AccountController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
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

            // Find user by username and password
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.DisplayName == username && u.PasswordHash == hashedPassword);

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

            // If return URL is specified and is local, redirect there
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

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
            if (await _context.Users.AnyAsync(u => u.DisplayName == displayName))
            {
                ViewBag.ErrorMessage = "Tên đăng nhập đã được sử dụng";
                return View();
            }

            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == email))
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

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

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
        public async Task<IActionResult> Profile()
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            // Get user details
            var user = await _context.Users
                .Include(u => u.UserAvatarFrames)
                .ThenInclude(uaf => uaf.AvatarFrame)
                .FirstOrDefaultAsync(u => u.Id == userId);
                
            if (user == null)
            {
                return NotFound();
            }

            // Get all available avatar frames
            var allFrames = await _context.AvatarFrames.ToListAsync();
            ViewBag.AllAvatarFrames = allFrames;
            
            // Get user's equipped frame
            var equippedFrame = user.UserAvatarFrames?.FirstOrDefault(f => f.IsEquipped)?.AvatarFrame;
            ViewBag.EquippedFrame = equippedFrame;

            await LoadUserActivityHistory(user.Id);
            
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(string id, string displayName, string email, 
            string currentPassword, string newPassword, string confirmPassword, IFormFile AvatarFile)
        {
            // Get user details
            var user = await _context.Users.FindAsync(id);
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
                if (await _context.Users.AnyAsync(u => u.DisplayName == displayName && u.Id != id))
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

            _context.Update(user);
            await _context.SaveChangesAsync();

            ViewBag.SuccessMessage = "Thông tin tài khoản đã được cập nhật thành công";
            
            // Load user activity history for the profile view
            await LoadUserActivityHistory(user.Id);
            
            return View("Profile", user);
        }

        private async Task LoadUserActivityHistory(string userId)
        {
            try
            {
                // Load recent downloads - chỉ select các cột cần thiết
                ViewBag.RecentDownloads = await _context.DownloadHistory
                    .Include(d => d.Game)
                    .Where(d => d.UserId == userId)
                    .OrderByDescending(d => d.DownloadDate)
                    .Select(d => new
                    {
                        d.Id,
                        d.GameId,
                        d.DownloadDate,
                        Game = d.Game
                    })
                    .Take(10)
                    .ToListAsync();

                // Load user reviews
                ViewBag.UserReviews = await _context.Reviews
                    .Include(r => r.Game)
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.DatePosted)
                    .Take(10)
                    .ToListAsync();

                // Load search history
                ViewBag.SearchHistory = await _context.SearchHistory
                    .Where(s => s.UserId == userId)
                    .OrderByDescending(s => s.SearchDate)
                    .Take(10)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Ghi log lỗi và khởi tạo các ViewBag rỗng để tránh lỗi null
                Console.WriteLine($"Error in LoadUserActivityHistory: {ex.Message}");
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
            var favorites = await _context.FavoriteGames
                .Include(f => f.Game)
                .ThenInclude(g => g.Category!)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.DateAdded)
                .ToListAsync();

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
            catch (Exception ex)
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
                // First, unequip all currently equipped frames for the user
                var userFrames = await _context.UserAvatarFrames
                    .Where(f => f.UserId == userId && f.IsEquipped)
                    .ToListAsync();
                
                foreach (var frame in userFrames)
                {
                    frame.IsEquipped = false;
                }
                
                // Check if the user has this frame
                var userFrame = await _context.UserAvatarFrames
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.FrameId == frameId);
                
                if (userFrame != null)
                {
                    // User has this frame, equip it
                    userFrame.IsEquipped = true;
                }
                else
                {
                    // User doesn't have this frame, check if it's a free frame
                    var frame = await _context.AvatarFrames.FindAsync(frameId);
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
                    _context.UserAvatarFrames.Add(userFrame);
                }
                
                await _context.SaveChangesAsync();
                
                var equippedFrame = await _context.AvatarFrames.FindAsync(frameId);
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
                // Unequip all currently equipped frames for the user
                var userFrames = await _context.UserAvatarFrames
                    .Where(f => f.UserId == userId && f.IsEquipped)
                    .ToListAsync();
                
                foreach (var frame in userFrames)
                {
                    frame.IsEquipped = false;
                }
                
                await _context.SaveChangesAsync();
                
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