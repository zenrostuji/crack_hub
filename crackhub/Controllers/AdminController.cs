using crackhub.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crackhub.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int PageSize = 10; // Number of items per page

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin Dashboard
        public async Task<IActionResult> Index()
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            // Get statistics for dashboard
            var stats = new Dictionary<string, int>
            {
                { "TotalGames", await _context.Games.CountAsync() },
                { "TotalUsers", await _context.Users.CountAsync() },
                { "TotalDownloads", await _context.DownloadHistory.CountAsync() },
                { "TotalCategories", await _context.Categories.CountAsync() }
            };

            // Get recent activities
            var recentGames = await _context.Games
                .OrderByDescending(g => g.Id)
                .Take(5)
                .ToListAsync();

            var recentUsers = await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .Take(5)
                .ToListAsync();

            // Calculate average rating across all games
            var averageRating = await _context.Games
                .Where(g => g.AverageRating > 0)
                .AverageAsync(g => g.AverageRating);

            // Handle the case where no games have ratings
            if (double.IsNaN(averageRating))
            {
                averageRating = 0;
            }

            // Get popular games (most downloaded)
            var popularGames = await _context.Games
                .OrderByDescending(g => g.Downloads)
                .Take(5)
                .ToListAsync();

            // Get recent downloads - fix for the IP column issue
            var recentDownloads = await _context.DownloadHistory
                .Include(d => d.Game)
                .Include(d => d.User)
                .OrderByDescending(d => d.DownloadDate)
                .Take(5)
                .Select(d => new DownloadHistory
                {
                    Id = d.Id,
                    UserId = d.UserId,
                    GameId = d.GameId,
                    DownloadDate = d.DownloadDate,
                    User = d.User,
                    Game = d.Game
                    // Explicitly not selecting the IP field
                })
                .ToListAsync();

            ViewBag.TotalGames = stats["TotalGames"];
            ViewBag.TotalUsers = stats["TotalUsers"];
            ViewBag.TotalDownloads = stats["TotalDownloads"];
            ViewBag.AverageRating = averageRating;
            ViewBag.RecentGames = recentGames;
            ViewBag.RecentUsers = recentUsers;
            ViewBag.RecentDownloads = recentDownloads;
            ViewBag.PopularGames = popularGames;

            return View();
        }

        #region Game Management

        // List all games with pagination
        public async Task<IActionResult> Games(int page = 1, string searchTerm = "", string sortOrder = "newest")
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            // Create query
            IQueryable<Game> gamesQuery = _context.Games.Include(g => g.Category);

            // Apply search if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                gamesQuery = gamesQuery.Where(g => g.Title.Contains(searchTerm) ||
                                                   g.ShortDescription.Contains(searchTerm) ||
                                                   g.Developer.Contains(searchTerm) ||
                                                   g.Publisher.Contains(searchTerm));
                ViewBag.SearchTerm = searchTerm;
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "title":
                    gamesQuery = gamesQuery.OrderBy(g => g.Title);
                    break;
                case "title_desc":
                    gamesQuery = gamesQuery.OrderByDescending(g => g.Title);
                    break;
                case "downloads":
                    gamesQuery = gamesQuery.OrderByDescending(g => g.Downloads);
                    break;
                case "rating":
                    gamesQuery = gamesQuery.OrderByDescending(g => g.AverageRating);
                    break;
                default: // newest
                    gamesQuery = gamesQuery.OrderByDescending(g => g.Id);
                    break;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParam = sortOrder == "title" ? "title_desc" : "title";

            // Count total games for pagination
            var totalGames = await gamesQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalGames / PageSize);

            // Get games for current page
            var games = await gamesQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(games);
        }

        // GET: Show create game form
        public async Task<IActionResult> CreateGame()
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            // Get categories for dropdown
            ViewBag.Categories = await _context.Categories.ToListAsync();
            // Get tags for multiselect
            ViewBag.Tags = await _context.Tags.ToListAsync();

            return View();
        }

        // POST: Create a new game
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGame(Game game, IFormFile coverImage, List<int> tagIds)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Handle cover image upload if provided
                if (coverImage != null && coverImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "games");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Create unique filename
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(coverImage.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await coverImage.CopyToAsync(fileStream);
                    }

                    // Set cover image URL
                    game.CoverImageUrl = "/img/games/" + uniqueFileName;
                }

                // Save the game
                _context.Games.Add(game);
                await _context.SaveChangesAsync();

                // Add tags if selected
                if (tagIds != null && tagIds.Count > 0)
                {
                    foreach (var tagId in tagIds)
                    {
                        var gameTag = new GameTag
                        {
                            GameId = game.Id,
                            TagId = tagId
                        };
                        _context.GameTags.Add(gameTag);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Games));
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            return View(game);
        }

        // GET: Show edit game form
        public async Task<IActionResult> EditGame(int? id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.GameTags)
                .FirstOrDefaultAsync(g => g.Id == id);
            
            if (game == null)
            {
                return NotFound();
            }

            // Get categories for dropdown
            ViewBag.Categories = await _context.Categories.ToListAsync();
            // Get tags for multiselect
            ViewBag.Tags = await _context.Tags.ToListAsync();
            // Get selected tag IDs
            ViewBag.SelectedTagIds = game.GameTags.Select(gt => gt.TagId).ToList();

            return View(game);
        }

        // POST: Update a game
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGame(Game game, IFormFile coverImage, List<int> tagIds)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing game with all properties
                    var existingGame = await _context.Games.FindAsync(game.Id);
                    if (existingGame == null)
                    {
                        return NotFound();
                    }

                    // Lưu đường dẫn ảnh cũ trước để sau này gán lại nếu không có ảnh mới
                    string existingCoverImageUrl = existingGame.CoverImageUrl;

                    // Handle cover image upload if provided
                    if (coverImage != null && coverImage.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "games");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Create unique filename
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(coverImage.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save the file
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await coverImage.CopyToAsync(fileStream);
                        }

                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(existingCoverImageUrl) && 
                            !existingCoverImageUrl.Contains("no-image.jpg") && 
                            existingCoverImageUrl.Contains("games"))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", 
                                existingCoverImageUrl.TrimStart('/').Replace('/', '\\'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Set new cover image URL
                        game.CoverImageUrl = "/img/games/" + uniqueFileName;
                    }
                    else
                    {
                        // Keep existing cover image - this is important for when no new image is uploaded
                        game.CoverImageUrl = existingCoverImageUrl;
                    }
                    
                    // Cập nhật từng thuộc tính thay vì sử dụng SetValues
                    existingGame.Title = game.Title;
                    existingGame.Developer = game.Developer; 
                    existingGame.Publisher = game.Publisher;
                    existingGame.ReleaseDate = game.ReleaseDate;
                    existingGame.CategoryId = game.CategoryId;
                    existingGame.ShortDescription = game.ShortDescription;
                    existingGame.FullDescription = game.FullDescription;
                    existingGame.Size = game.Size;
                    existingGame.Rating = game.Rating;
                    existingGame.AverageRating = game.AverageRating;
                    existingGame.Downloads = game.Downloads;
                    existingGame.DownloadUrl = game.DownloadUrl;
                    existingGame.CoverImageUrl = game.CoverImageUrl; // Dùng CoverImageUrl đã xử lý ở trên
                    
                    _context.Update(existingGame);
                    
                    // Update tags
                    var existingTags = await _context.GameTags
                        .Where(gt => gt.GameId == game.Id)
                        .ToListAsync();

                    // Remove all existing tags
                    _context.GameTags.RemoveRange(existingTags);

                    // Add new tags
                    if (tagIds != null && tagIds.Count > 0)
                    {
                        foreach (var tagId in tagIds)
                        {
                            var gameTag = new GameTag
                            {
                                GameId = game.Id,
                                TagId = tagId
                            };
                            _context.GameTags.Add(gameTag);
                        }
                    }

                    await _context.SaveChangesAsync();
                    
                    // Thêm thông báo thành công
                    TempData["SuccessMessage"] = "Game đã được cập nhật thành công!";
                    
                    return RedirectToAction(nameof(Games));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi và thêm vào ModelState để hiển thị
                    ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            ViewBag.SelectedTagIds = tagIds;
            
            // Hiển thị thông báo lỗi
            TempData["ErrorMessage"] = "Có lỗi xảy ra khi lưu thông tin game!";
            
            return View(game);
        }

        // GET: Confirm delete game
        public async Task<IActionResult> DeleteGame(int? id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Category)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Delete a game
        [HttpPost, ActionName("DeleteGame")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGameConfirmed(int ? id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                // Delete cover image if exists
                if (!string.IsNullOrEmpty(game.CoverImageUrl))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", 
                        game.CoverImageUrl.TrimStart('/').Replace('/', '\\'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                // Delete related data
                var screenshots = await _context.Screenshots.Where(s => s.GameId == id).ToListAsync();
                _context.Screenshots.RemoveRange(screenshots);

                var features = await _context.Features.Where(f => f.GameId == id).ToListAsync();
                _context.Features.RemoveRange(features);

                var systemRequirements = await _context.SystemRequirements.Where(sr => sr.GameId == id).ToListAsync();
                _context.SystemRequirements.RemoveRange(systemRequirements);

                var reviews = await _context.Reviews.Where(r => r.GameId == id).ToListAsync();
                _context.Reviews.RemoveRange(reviews);

                var favoriteGames = await _context.FavoriteGames.Where(fg => fg.GameId == id).ToListAsync();
                _context.FavoriteGames.RemoveRange(favoriteGames);

                // Fix: select only necessary columns to avoid IP column issue
                var downloadHistory = await _context.DownloadHistory
                    .Where(dh => dh.GameId == id)
                    .Select(dh => new DownloadHistory
                    {
                        Id = dh.Id,
                        UserId = dh.UserId,
                        GameId = dh.GameId,
                        DownloadDate = dh.DownloadDate
                    })
                    .ToListAsync();
                _context.DownloadHistory.RemoveRange(downloadHistory);

                var gameLinks = await _context.GameLinks.Where(gl => gl.GameId == id).ToListAsync();
                _context.GameLinks.RemoveRange(gameLinks);

                var gameTags = await _context.GameTags.Where(gt => gt.GameId == id).ToListAsync();
                _context.GameTags.RemoveRange(gameTags);

                try
                {
                    var crackInfos = await _context.CrackInfo.Where(c => c.GameId == id).ToListAsync();
                    _context.CrackInfo.RemoveRange(crackInfos);
                }
                catch { /* Table might not exist */ }

                try
                {
                    var localizations = await _context.LocalizationInfos.Where(l => l.GameId == id).ToListAsync();
                    _context.LocalizationInfos.RemoveRange(localizations);
                }
                catch { /* Table might not exist */ }

                try
                {
                    var relatedGames = await _context.RelatedGames.Where(rg => rg.GameId == id || rg.RelatedGameId == id).ToListAsync();
                    _context.RelatedGames.RemoveRange(relatedGames);
                }
                catch { /* Table might not exist */ }

                // Finally delete the game
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Games));
        }
        

        // GET: Manage game screenshots
        public async Task<IActionResult> ManageScreenshots(int? id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Screenshots)
                .FirstOrDefaultAsync(g => g.Id == id);
                
            if (game == null)
            {
                return NotFound();
            }

            ViewBag.Game = game;
            return View(game.Screenshots.ToList());
        }

        // POST: Add screenshot
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddScreenshot(int gameId, IFormFile imageFile)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (imageFile == null || imageFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please select an image file.";
                return RedirectToAction(nameof(ManageScreenshots), new { id = gameId });
            }

            var game = await _context.Games.FindAsync(gameId);
            if (game == null)
            {
                return NotFound();
            }

            // Upload image
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "games", "screenshots");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Create unique filename
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Create screenshot
            var screenshot = new Screenshot
            {
                GameId = gameId,
                ImageUrl = "/img/games/screenshots/" + uniqueFileName,
                //Caption = Path.GetFileNameWithoutExtension(imageFile.FileName)
            };

            _context.Screenshots.Add(screenshot);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageScreenshots), new { id = gameId });
        }

        // POST: Delete screenshot
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteScreenshot(int id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var screenshot = await _context.Screenshots.FindAsync(id);
            if (screenshot == null)
            {
                return NotFound();
            }

            var gameId = screenshot.GameId;

            // Delete image file if exists
            if (!string.IsNullOrEmpty(screenshot.ImageUrl))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", 
                    screenshot.ImageUrl.TrimStart('/').Replace('/', '\\'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Screenshots.Remove(screenshot);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageScreenshots), new { id = gameId });
        }

        // GET: Manage game links
        public async Task<IActionResult> ManageLinks(int? id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.GameLinks)
                .FirstOrDefaultAsync(g => g.Id == id);
                
            if (game == null)
            {
                return NotFound();
            }

            ViewBag.Game = game;
            return View(game.GameLinks.ToList());
        }

        // POST: Add game link
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGameLink(GameLink gameLink)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                gameLink.DateAdded = DateTime.Now;
                _context.GameLinks.Add(gameLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageLinks), new { id = gameLink.GameId });
            }

            var game = await _context.Games
                .Include(g => g.GameLinks)
                .FirstOrDefaultAsync(g => g.Id == gameLink.GameId);
                
            ViewBag.Game = game;
            return View(nameof(ManageLinks), game.GameLinks.ToList());
        }

        // POST: Delete game link
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGameLink(int id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var gameLink = await _context.GameLinks.FindAsync(id);
            if (gameLink == null)
            {
                return NotFound();
            }

            var gameId = gameLink.GameId;

            _context.GameLinks.Remove(gameLink);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageLinks), new { id = gameId });
        }

        #endregion

        #region User Management

        // List all users with pagination
        public async Task<IActionResult> Users(int page = 1, string searchTerm = "", string sortOrder = "newest")
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            // Create query
            IQueryable<User> usersQuery = _context.Users
                .Include(u => u.Role)
                .Include(u => u.Reviews); // Include Reviews collection to properly count them

            // Apply search if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                usersQuery = usersQuery.Where(u => u.DisplayName.Contains(searchTerm) ||
                                                  u.Email.Contains(searchTerm) ||
                                                  u.FirstName.Contains(searchTerm) ||
                                                  u.LastName.Contains(searchTerm));
                ViewBag.SearchTerm = searchTerm;
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "name":
                    usersQuery = usersQuery.OrderBy(u => u.DisplayName);
                    break;
                case "name_desc":
                    usersQuery = usersQuery.OrderByDescending(u => u.DisplayName);
                    break;
                case "email":
                    usersQuery = usersQuery.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    usersQuery = usersQuery.OrderByDescending(u => u.Email);
                    break;
                case "role":
                    usersQuery = usersQuery.OrderBy(u => u.RoleId);
                    break;
                case "role_desc":
                    usersQuery = usersQuery.OrderByDescending(u => u.RoleId);
                    break;
                default: // newest
                    usersQuery = usersQuery.OrderByDescending(u => u.CreatedAt);
                    break;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.EmailSortParam = sortOrder == "email" ? "email_desc" : "email";
            ViewBag.RoleSortParam = sortOrder == "role" ? "role_desc" : "role";

            // Count total users for pagination
            var totalUsers = await usersQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalUsers / PageSize);

            // Get users for current page
            var users = await usersQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Roles = await _context.Roles.ToListAsync();

            return View(users);
        }

        // GET: Show create user form
        public async Task<IActionResult> CreateUser()
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Roles = await _context.Roles.ToListAsync();
            return View();
        }

        // POST: Create a new user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(User user, string password, IFormFile avatarFile)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Check if username already exists
                if (await _context.Users.AnyAsync(u => u.DisplayName == user.DisplayName))
                {
                    ModelState.AddModelError("DisplayName", "Username already exists");
                    ViewBag.Roles = await _context.Roles.ToListAsync();
                    return View(user);
                }

                // Check if email already exists
                if (!string.IsNullOrEmpty(user.Email) && await _context.Users.AnyAsync(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    ViewBag.Roles = await _context.Roles.ToListAsync();
                    return View(user);
                }

                // Generate a new ID
                user.Id = Guid.NewGuid().ToString();
                // Set password hash
                user.PasswordHash = HashPassword(password);
                // Set created date
                user.CreatedAt = DateTime.Now;

                // Handle avatar upload if provided
                if (avatarFile != null && avatarFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "avartars");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Use DisplayName as filename, ensure no invalid characters
                    string fileName = user.DisplayName.Trim();
                    fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
                    string uniqueFileName = $"{fileName}.jpg";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatarFile.CopyToAsync(fileStream);
                    }

                    // Set avatar URL
                    user.AvatarUrl = $"/img/avartars/{uniqueFileName}";
                }

                // Save the user
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Users));
            }

            // If we got this far, something failed
            ViewBag.Roles = await _context.Roles.ToListAsync();
            return View(user);
        }

        // GET: Show edit user form
        public async Task<IActionResult> EditUser(string id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Roles = await _context.Roles.ToListAsync();
            return View(user);
        }

        // POST: Update a user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(User user, string newPassword, IFormFile avatarFile)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Get existing user from database
                var existingUser = await _context.Users.FindAsync(user.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                // Check username uniqueness if changed
                if (existingUser.DisplayName != user.DisplayName && 
                    await _context.Users.AnyAsync(u => u.DisplayName == user.DisplayName))
                {
                    ModelState.AddModelError("DisplayName", "Username already exists");
                    ViewBag.Roles = await _context.Roles.ToListAsync();
                    return View(user);
                }

                // Check email uniqueness if changed
                if (!string.IsNullOrEmpty(user.Email) && existingUser.Email != user.Email && 
                    await _context.Users.AnyAsync(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    ViewBag.Roles = await _context.Roles.ToListAsync();
                    return View(user);
                }

                // Update password if provided
                if (!string.IsNullOrEmpty(newPassword))
                {
                    existingUser.PasswordHash = HashPassword(newPassword);
                }

                // Handle avatar upload if provided
                if (avatarFile != null && avatarFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "avartars");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Use DisplayName as filename, ensure no invalid characters
                    string fileName = user.DisplayName.Trim();
                    fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
                    string uniqueFileName = $"{fileName}.jpg";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // If file exists, delete it
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    // Save the file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatarFile.CopyToAsync(fileStream);
                    }

                    // Set avatar URL
                    existingUser.AvatarUrl = $"/img/avartars/{uniqueFileName}";
                }

                // Update user properties
                existingUser.DisplayName = user.DisplayName;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.RoleId = user.RoleId;
                existingUser.Bio = user.Bio;
                existingUser.PremiumExpiryDate = user.PremiumExpiryDate;
                existingUser.EmailConfirmed = user.EmailConfirmed;
                existingUser.NormalizedEmail = user.Email?.ToUpper();
                existingUser.RememberMe = user.RememberMe;
                existingUser.SecurityStamp = user.SecurityStamp;

                // Update the entity using Entry method
                _context.Entry(existingUser).State = EntityState.Modified;
                
                // Save changes to database
                var result = await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = $"Thông tin người dùng đã được cập nhật thành công! ({result} records affected)";
                return RedirectToAction(nameof(Users));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                // Log lỗi và thêm vào ModelState để hiển thị
                ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                TempData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
            }

            // If we got this far, something failed
            ViewBag.Roles = await _context.Roles.ToListAsync();
            return View(user);
        }

        // POST: Delete a user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
                
            if (user == null)
            {
                return NotFound();
            }

            // Kiểm tra nếu đây là tài khoản Admin duy nhất
            if (user.RoleId == 3)
            {
                var adminCount = await _context.Users.CountAsync(u => u.RoleId == 3);
                if (adminCount <= 1)
                {
                    ViewBag.ErrorMessage = "Không thể xóa tài khoản Admin duy nhất.";
                    return View(user);
                }
            }

            // Xóa avatar nếu có
            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", 
                    user.AvatarUrl.TrimStart('/').Replace('/', '\\'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Xóa dữ liệu liên quan
            var reviews = await _context.Reviews.Where(r => r.UserId == id).ToListAsync();
            _context.Reviews.RemoveRange(reviews);

            var favoriteGames = await _context.FavoriteGames.Where(fg => fg.UserId == id).ToListAsync();
            _context.FavoriteGames.RemoveRange(favoriteGames);

            // Fix: select only necessary columns to avoid IP column issue
            var downloadHistory = await _context.DownloadHistory
                .Where(dh => dh.UserId == id)
                .Select(dh => new DownloadHistory
                {
                    Id = dh.Id,
                    UserId = dh.UserId,
                    GameId = dh.GameId,
                    DownloadDate = dh.DownloadDate
                })
                .ToListAsync();
            _context.DownloadHistory.RemoveRange(downloadHistory);

            // Fix: select only necessary columns to avoid SearchQuery column issue
            var searchHistory = await _context.SearchHistory
                .Where(sh => sh.UserId == id)
                .Select(sh => new SearchHistory
                {
                    Id = sh.Id,
                    UserId = sh.UserId,
                    SearchDate = sh.SearchDate
                })
                .ToListAsync();
            _context.SearchHistory.RemoveRange(searchHistory);

            // Xóa người dùng
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Users));
        }

        // GET: View user details
        public async Task<IActionResult> UserDetails(string id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
                
            if (user == null)
            {
                return NotFound();
            }

            // Get user activity
            var recentDownloads = await _context.DownloadHistory
                .Include(d => d.Game)
                .Where(d => d.UserId == id)
                .OrderByDescending(d => d.DownloadDate)
                .Take(10)
                .Select(d => new DownloadHistory
                {
                    Id = d.Id,
                    UserId = d.UserId,
                    GameId = d.GameId,
                    DownloadDate = d.DownloadDate,
                    User = d.User,
                    Game = d.Game
                    // Explicitly not selecting the IP field
                })
                .ToListAsync();


            var favoriteGames = await _context.FavoriteGames
                .Include(f => f.Game)
                .Where(f => f.UserId == id)
                .OrderByDescending(f => f.DateAdded)
                .Take(10)
                .ToListAsync();

            var reviews = await _context.Reviews
                .Include(r => r.Game)
                .Where(r => r.UserId == id)
                .OrderByDescending(r => r.DatePosted)
                .Take(10)
                .ToListAsync();

            ViewBag.RecentDownloads = recentDownloads;
            ViewBag.FavoriteGames = favoriteGames;
            ViewBag.Reviews = reviews;

            return View(user);
        }

        #endregion

        #region System Requirements Management

        // GET: Manage game system requirements
        public async Task<IActionResult> ManageSystemRequirements(int? id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var systemRequirements = await _context.SystemRequirements
                .Where(sr => sr.GameId == id)
                .ToListAsync();

            ViewBag.Game = game;
            return View(systemRequirements);
        }

        // POST: Save system requirement
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSystemRequirement(SystemRequirement systemRequirement)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                if (systemRequirement.Id > 0)
                {
                    // Update existing requirement
                    _context.Update(systemRequirement);
                    TempData["SuccessMessage"] = "Cập nhật cấu hình thành công!";
                }
                else
                {
                    // Create new requirement
                    // Check if a requirement of this type already exists for the game
                    var existingRequirement = await _context.SystemRequirements
                        .FirstOrDefaultAsync(sr => sr.GameId == systemRequirement.GameId && 
                                            sr.RequirementType == systemRequirement.RequirementType);

                    if (existingRequirement != null)
                    {
                        // Update existing requirement
                        existingRequirement.OS = systemRequirement.OS;
                        existingRequirement.Processor = systemRequirement.Processor;
                        existingRequirement.Memory = systemRequirement.Memory;
                        existingRequirement.Graphics = systemRequirement.Graphics;
                        existingRequirement.DirectX = systemRequirement.DirectX;
                        existingRequirement.Storage = systemRequirement.Storage;
                        _context.Update(existingRequirement);
                    }
                    else
                    {
                        // Create new requirement
                        _context.Add(systemRequirement);
                    }
                    TempData["SuccessMessage"] = "Thêm cấu hình thành công!";
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageSystemRequirements), new { id = systemRequirement.GameId });
            }

            TempData["ErrorMessage"] = "Có lỗi xảy ra, vui lòng kiểm tra lại dữ liệu nhập vào!";
            return RedirectToAction(nameof(ManageSystemRequirements), new { id = systemRequirement.GameId });
        }

        #endregion

        #region Crack Info Management

        // GET: Manage game crack info
        public async Task<IActionResult> ManageCrackInfos(int? id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var crackInfos = await _context.CrackInfo
                .Where(ci => ci.GameId == id)
                .ToListAsync();

            ViewBag.Game = game;
            return View(crackInfos);
        }

        // POST: Add crack info
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCrackInfo(CrackInfo crackInfo)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Set recommended if this is the first crack info for the game
                var anyExisting = await _context.CrackInfo.AnyAsync(ci => ci.GameId == crackInfo.GameId);
                if (!anyExisting)
                {
                    crackInfo.IsRecommended = true;
                }
                else if (crackInfo.IsRecommended)
                {
                    // If this is marked as recommended, unmark others
                    var existingRecommended = await _context.CrackInfo
                        .Where(ci => ci.GameId == crackInfo.GameId && ci.IsRecommended)
                        .ToListAsync();

                    foreach (var ci in existingRecommended)
                    {
                        ci.IsRecommended = false;
                        _context.Update(ci);
                    }
                }

                _context.Add(crackInfo);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm thông tin crack thành công!";
                return RedirectToAction(nameof(ManageCrackInfos), new { id = crackInfo.GameId });
            }

            TempData["ErrorMessage"] = "Có lỗi xảy ra, vui lòng kiểm tra lại dữ liệu nhập vào!";
            var game = await _context.Games.FindAsync(crackInfo.GameId);
            ViewBag.Game = game;
            
            var crackInfos = await _context.CrackInfo
                .Where(ci => ci.GameId == crackInfo.GameId)
                .ToListAsync();
                
            return View(nameof(ManageCrackInfos), crackInfos);
        }

        // POST: Edit crack info
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCrackInfo(CrackInfo crackInfo)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get existing crack info
                    var existingCrackInfo = await _context.CrackInfo.FindAsync(crackInfo.Id);
                    if (existingCrackInfo == null)
                    {
                        return NotFound();
                    }

                    // If this is marked as recommended, unmark others
                    if (crackInfo.IsRecommended && !existingCrackInfo.IsRecommended)
                    {
                        var existingRecommended = await _context.CrackInfo
                            .Where(ci => ci.GameId == crackInfo.GameId && ci.IsRecommended)
                            .ToListAsync();

                        foreach (var ci in existingRecommended)
                        {
                            ci.IsRecommended = false;
                            _context.Update(ci);
                        }
                    }

                    // Update properties
                    existingCrackInfo.Version = crackInfo.Version;
                    existingCrackInfo.Group = crackInfo.Group;
                    existingCrackInfo.Description = crackInfo.Description;
                    existingCrackInfo.DownloadUrl = crackInfo.DownloadUrl;
                    existingCrackInfo.FileSize = crackInfo.FileSize;
                    existingCrackInfo.ReleaseDate = crackInfo.ReleaseDate;
                    existingCrackInfo.IsRecommended = crackInfo.IsRecommended;

                    _context.Update(existingCrackInfo);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cập nhật thông tin crack thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CrackInfoExists(crackInfo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ManageCrackInfos), new { id = crackInfo.GameId });
            }

            TempData["ErrorMessage"] = "Có lỗi xảy ra, vui lòng kiểm tra lại dữ liệu nhập vào!";
            return RedirectToAction(nameof(ManageCrackInfos), new { id = crackInfo.GameId });
        }

        // POST: Delete crack info
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCrackInfo(int id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var crackInfo = await _context.CrackInfo.FindAsync(id);
            if (crackInfo == null)
            {
                return NotFound();
            }

            var gameId = crackInfo.GameId;
            var wasRecommended = crackInfo.IsRecommended;

            _context.CrackInfo.Remove(crackInfo);
            await _context.SaveChangesAsync();

            // If this was the recommended crack, set another one as recommended if any exist
            if (wasRecommended)
            {
                var anotherCrack = await _context.CrackInfo
                    .FirstOrDefaultAsync(ci => ci.GameId == gameId);

                if (anotherCrack != null)
                {
                    anotherCrack.IsRecommended = true;
                    _context.Update(anotherCrack);
                    await _context.SaveChangesAsync();
                }
            }

            TempData["SuccessMessage"] = "Xóa thông tin crack thành công!";
            return RedirectToAction(nameof(ManageCrackInfos), new { id = gameId });
        }

        // POST: Set a crack info as recommended
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetRecommendedCrack(int id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var crackInfo = await _context.CrackInfo.FindAsync(id);
            if (crackInfo == null)
            {
                return NotFound();
            }

            // Unmark all other crack infos for this game
            var otherCracks = await _context.CrackInfo
                .Where(ci => ci.GameId == crackInfo.GameId && ci.Id != id && ci.IsRecommended)
                .ToListAsync();

            foreach (var crack in otherCracks)
            {
                crack.IsRecommended = false;
                _context.Update(crack);
            }

            // Mark this one as recommended
            crackInfo.IsRecommended = true;
            _context.Update(crackInfo);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đặt crack khuyên dùng thành công!";
            return RedirectToAction(nameof(ManageCrackInfos), new { id = crackInfo.GameId });
        }

        #endregion

        #region Feature Management

        // GET: Manage game features
        public async Task<IActionResult> ManageFeatures(int? id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var features = await _context.Features
                .Where(f => f.GameId == id)
                .ToListAsync();

            ViewBag.Game = game;
            return View(features);
        }

        // POST: Add feature
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFeature(Feature feature)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                _context.Add(feature);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm tính năng thành công!";
                return RedirectToAction(nameof(ManageFeatures), new { id = feature.GameId });
            }

            TempData["ErrorMessage"] = "Có lỗi xảy ra, vui lòng kiểm tra lại dữ liệu nhập vào!";
            return RedirectToAction(nameof(ManageFeatures), new { id = feature.GameId });
        }

        // POST: Edit feature
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFeature(Feature feature)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feature);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật tính năng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeatureExists(feature.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ManageFeatures), new { id = feature.GameId });
            }

            TempData["ErrorMessage"] = "Có lỗi xảy ra, vui lòng kiểm tra lại dữ liệu nhập vào!";
            return RedirectToAction(nameof(ManageFeatures), new { id = feature.GameId });
        }

        // POST: Delete feature
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var feature = await _context.Features.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }

            var gameId = feature.GameId;

            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa tính năng thành công!";
            return RedirectToAction(nameof(ManageFeatures), new { id = gameId });
        }

        #endregion

        #region Category Management

        // List all categories with pagination
        public async Task<IActionResult> Categories(int page = 1, string searchTerm = "", string sortOrder = "name")
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            // Create query
            IQueryable<Category> categoriesQuery = _context.Categories;

            // Apply search if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                categoriesQuery = categoriesQuery.Where(c => c.CategoryName.Contains(searchTerm) ||
                                                      c.Description.Contains(searchTerm));
                ViewBag.SearchTerm = searchTerm;
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "name_desc":
                    categoriesQuery = categoriesQuery.OrderByDescending(c => c.CategoryName);
                    break;
                case "games":
                    categoriesQuery = categoriesQuery.OrderByDescending(c => c.GameCount);
                    break;
                case "games_asc":
                    categoriesQuery = categoriesQuery.OrderBy(c => c.GameCount);
                    break;
                default: // name
                    categoriesQuery = categoriesQuery.OrderBy(c => c.CategoryName);
                    break;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.GamesSortParam = sortOrder == "games" ? "games_asc" : "games";

            // Count total categories for pagination
            var totalCategories = await categoriesQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCategories / PageSize);

            // Get categories for current page
            var categories = await categoriesQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(categories);
        }

        // GET: Show create category form
        public IActionResult CreateCategory()
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Create a new category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Generate slug if not provided
                if (string.IsNullOrEmpty(category.Slug))
                {
                    category.Slug = category.CategoryName.ToLower().Replace(" ", "-");
                }

                // Check if slug already exists
                if (await _context.Categories.AnyAsync(c => c.Slug == category.Slug))
                {
                    // Append a number to the slug to make it unique
                    var baseSlug = category.Slug;
                    var slugCount = 1;
                    while (await _context.Categories.AnyAsync(c => c.Slug == category.Slug))
                    {
                        category.Slug = $"{baseSlug}-{slugCount}";
                        slugCount++;
                    }
                }

                _context.Add(category);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Thể loại đã được tạo thành công!";
                return RedirectToAction(nameof(Categories));
            }

            return View(category);
        }

        // GET: Show edit category form
        public async Task<IActionResult> EditCategory(int? id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Update a category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(Category category)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Generate slug if not provided
                    if (string.IsNullOrEmpty(category.Slug))
                    {
                        category.Slug = category.CategoryName.ToLower().Replace(" ", "-");
                    }

                    // Check if slug already exists for other categories
                    var existingCategory = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Slug == category.Slug && c.CategoryId != category.CategoryId);

                    if (existingCategory != null)
                    {
                        // Append a number to the slug to make it unique
                        var baseSlug = category.Slug;
                        var slugCount = 1;
                        while (await _context.Categories.AnyAsync(c => c.Slug == category.Slug && c.CategoryId != category.CategoryId))
                        {
                            category.Slug = $"{baseSlug}-{slugCount}";
                            slugCount++;
                        }
                    }

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Thể loại đã được cập nhật thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Categories));
            }
            return View(category);
        }

        // GET: Confirm delete category
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            // Check if category has games
            var gamesCount = await _context.Games.CountAsync(g => g.CategoryId == id);
            ViewBag.HasGames = gamesCount > 0;
            ViewBag.GamesCount = gamesCount;

            return View(category);
        }

        // POST: Delete a category
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Check if category has games
            var hasGames = await _context.Games.AnyAsync(g => g.CategoryId == id);
            if (hasGames)
            {
                TempData["ErrorMessage"] = "Không thể xóa thể loại này vì có game thuộc thể loại này!";
                return RedirectToAction(nameof(Categories));
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thể loại đã được xóa thành công!";
            return RedirectToAction(nameof(Categories));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.CategoryId == id);
        }

        #endregion

        #region Tag Management

        // List all tags with pagination
        public async Task<IActionResult> Tags(int page = 1, string searchTerm = "", string sortOrder = "name")
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            // Create query
            IQueryable<Tag> tagsQuery = _context.Tags;

            // Apply search if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                tagsQuery = tagsQuery.Where(t => t.Name.Contains(searchTerm) || 
                                            (t.Slug != null && t.Slug.Contains(searchTerm)));
                ViewBag.SearchTerm = searchTerm;
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "name_desc":
                    tagsQuery = tagsQuery.OrderByDescending(t => t.Name);
                    break;
                default: // name
                    tagsQuery = tagsQuery.OrderBy(t => t.Name);
                    break;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "name" ? "name_desc" : "name";

            // Count total tags for pagination
            var totalTags = await tagsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalTags / PageSize);

            // Get tags for current page
            var tags = await tagsQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            // Count games for each tag
            var tagIdCounts = await _context.GameTags
                .GroupBy(gt => gt.TagId)
                .Select(g => new { TagId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.TagId, g => g.Count);

            ViewBag.TagGameCounts = tagIdCounts;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(tags);
        }

        // GET: Show create tag form
        public IActionResult CreateTag()
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Create a new tag
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTag(Tag tag)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Generate slug if not provided
                if (string.IsNullOrEmpty(tag.Slug))
                {
                    tag.Slug = tag.Name.ToLower().Replace(" ", "-");
                }

                // Check if name already exists
                if (await _context.Tags.AnyAsync(t => t.Name == tag.Name))
                {
                    ModelState.AddModelError("Name", "Tag với tên này đã tồn tại!");
                    return View(tag);
                }

                // Check if slug already exists
                if (await _context.Tags.AnyAsync(t => t.Slug == tag.Slug))
                {
                    // Append a number to the slug to make it unique
                    var baseSlug = tag.Slug;
                    var slugCount = 1;
                    while (await _context.Tags.AnyAsync(t => t.Slug == tag.Slug))
                    {
                        tag.Slug = $"{baseSlug}-{slugCount}";
                        slugCount++;
                    }
                }

                _context.Add(tag);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Tag đã được tạo thành công!";
                return RedirectToAction(nameof(Tags));
            }
            return View(tag);
        }

        // GET: Show edit tag form
        public async Task<IActionResult> EditTag(int? id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Update a tag
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTag(Tag tag)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Generate slug if not provided
                    if (string.IsNullOrEmpty(tag.Slug))
                    {
                        tag.Slug = tag.Name.ToLower().Replace(" ", "-");
                    }

                    // Check if name already exists for other tags
                    var existingNameTag = await _context.Tags
                        .FirstOrDefaultAsync(t => t.Name == tag.Name && t.Id != tag.Id);

                    if (existingNameTag != null)
                    {
                        ModelState.AddModelError("Name", "Tag với tên này đã tồn tại!");
                        return View(tag);
                    }

                    // Check if slug already exists for other tags
                    var existingSlugTag = await _context.Tags
                        .FirstOrDefaultAsync(t => t.Slug == tag.Slug && t.Id != tag.Id);

                    if (existingSlugTag != null)
                    {
                        // Append a number to the slug to make it unique
                        var baseSlug = tag.Slug;
                        var slugCount = 1;
                        while (await _context.Tags.AnyAsync(t => t.Slug == tag.Slug && t.Id != tag.Id))
                        {
                            tag.Slug = $"{baseSlug}-{slugCount}";
                            slugCount++;
                        }
                    }

                    _context.Update(tag);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Tag đã được cập nhật thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagExists(tag.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Tags));
            }
            return View(tag);
        }

        // GET: Confirm delete tag
        public async Task<IActionResult> DeleteTag(int? id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            // Check if tag has games
            var gameCount = await _context.GameTags.CountAsync(gt => gt.TagId == id);
            ViewBag.HasGames = gameCount > 0;
            ViewBag.GameCount = gameCount;

            return View(tag);
        }

        // POST: Delete a tag
        [HttpPost, ActionName("DeleteTag")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTagConfirmed(int id)
        {
            // Check if user is admin
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            // Delete all game-tag associations
            var gameTags = await _context.GameTags.Where(gt => gt.TagId == id).ToListAsync();
            _context.GameTags.RemoveRange(gameTags);

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Tag đã được xóa thành công!";
            return RedirectToAction(nameof(Tags));
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(t => t.Id == id);
        }

        #endregion

        #region Helper Methods
        
        // Check if current user is admin
        private bool IsAdmin()
        {
            var userRoleString = HttpContext.Session.GetString("UserRole");
            
            if (string.IsNullOrEmpty(userRoleString) || !int.TryParse(userRoleString, out int userRole))
                return false;
                
            return userRole == 3; // Admin role ID is 3
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(g => g.Id == id);
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(u => u.Id == id);
        }
        
        private string HashPassword(string password)
        {
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
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
        
        private bool CrackInfoExists(int id)
        {
            return _context.CrackInfo.Any(ci => ci.Id == id);
        }
        
        private bool FeatureExists(int id)
        {
            return _context.Features.Any(f => f.Id == id);
        }
        
        #endregion
    }
}