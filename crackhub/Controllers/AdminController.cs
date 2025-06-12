using crackhub.Models.Data;
using crackhub.Repositories;
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
        private readonly IGameRepository _gameRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IGameTagRepository _gameTagRepository;
        private readonly ISystemRequirementRepository _systemRequirementRepository;
        private readonly ICrackInfoRepository _crackInfoRepository;
        private readonly IFeatureRepository _featureRepository;
        private readonly IScreenshotRepository _screenshotRepository;
        private readonly IGameLinkRepository _gameLinkRepository;
        private readonly IDownloadHistoryRepository _downloadHistoryRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IFavoriteGameRepository _favoriteGameRepository;
        private readonly ISearchHistoryRepository _searchHistoryRepository;
        private readonly ILocalizationInfoRepository _localizationInfoRepository;
        private readonly IRelatedGameRepository _relatedGameRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly int PageSize = 10; // Number of items per page

        public AdminController(
            IGameRepository gameRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            ITagRepository tagRepository,
            IGameTagRepository gameTagRepository,
            ISystemRequirementRepository systemRequirementRepository,
            ICrackInfoRepository crackInfoRepository,
            IFeatureRepository featureRepository,
            IScreenshotRepository screenshotRepository,
            IGameLinkRepository gameLinkRepository,
            IDownloadHistoryRepository downloadHistoryRepository,
            IRoleRepository roleRepository,
            IReviewRepository reviewRepository,
            IFavoriteGameRepository favoriteGameRepository,
            ISearchHistoryRepository searchHistoryRepository,
            ILocalizationInfoRepository localizationInfoRepository,
            IRelatedGameRepository relatedGameRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _gameRepository = gameRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
            _gameTagRepository = gameTagRepository;
            _systemRequirementRepository = systemRequirementRepository;
            _crackInfoRepository = crackInfoRepository;
            _featureRepository = featureRepository;
            _screenshotRepository = screenshotRepository;
            _gameLinkRepository = gameLinkRepository;
            _downloadHistoryRepository = downloadHistoryRepository;
            _roleRepository = roleRepository;
            _reviewRepository = reviewRepository;
            _favoriteGameRepository = favoriteGameRepository;
            _searchHistoryRepository = searchHistoryRepository;
            _localizationInfoRepository = localizationInfoRepository;
            _relatedGameRepository = relatedGameRepository;
            _webHostEnvironment = webHostEnvironment;
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
                { "TotalGames", await _gameRepository.GetTotalGamesCountAsync() },
                { "TotalUsers", await _userRepository.GetTotalUsersCountAsync() },
                { "TotalDownloads", await _downloadHistoryRepository.GetTotalDownloadsCountAsync() },
                { "TotalCategories", await _categoryRepository.GetTotalCategoriesCountAsync() }
            };

            // Get recent activities
            var recentGames = (await _gameRepository.GetRecentGamesAsync(5)).ToList();

            var recentUsers = (await _userRepository.GetRecentUsersAsync(5)).ToList();

            // Calculate average rating across all games
            var averageRating = await _gameRepository.GetAverageRatingAsync();

            // Handle the case where no games have ratings
            if (double.IsNaN(averageRating))
            {
                averageRating = 0;
            }

            // Get popular games (most downloaded)
            var popularGames = (await _gameRepository.GetPopularGamesAsync(5)).ToList();

            // Get recent downloads
            var recentDownloads = (await _downloadHistoryRepository.GetRecentDownloadsAsync(5)).ToList();

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

            // Get all games with category information
            var allGames = (await _gameRepository.GetAllAsync()).ToList();

            // Apply search if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                allGames = allGames.Where(g => g.Title.Contains(searchTerm) ||
                                             (g.ShortDescription?.Contains(searchTerm) ?? false) ||
                                             (g.Developer?.Contains(searchTerm) ?? false) ||
                                             (g.Publisher?.Contains(searchTerm) ?? false)).ToList();
                ViewBag.SearchTerm = searchTerm;
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "title":
                    allGames = allGames.OrderBy(g => g.Title).ToList();
                    break;
                case "title_desc":
                    allGames = allGames.OrderByDescending(g => g.Title).ToList();
                    break;
                case "downloads":
                    allGames = allGames.OrderByDescending(g => g.Downloads).ToList();
                    break;
                case "rating":
                    allGames = allGames.OrderByDescending(g => g.AverageRating).ToList();
                    break;
                default: // newest
                    allGames = allGames.OrderByDescending(g => g.Id).ToList();
                    break;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParam = sortOrder == "title" ? "title_desc" : "title";

            // Count total games for pagination
            var totalGames = allGames.Count;
            var totalPages = (int)Math.Ceiling((double)totalGames / PageSize);

            // Get games for current page
            var games = allGames
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

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
            ViewBag.Categories = (await _categoryRepository.GetAllAsync()).ToList();
            // Get tags for multiselect
            ViewBag.Tags = (await _tagRepository.GetAllAsync()).ToList();

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
                var createdGame = await _gameRepository.CreateAsync(game);

                // Add tags if selected
                if (tagIds != null && tagIds.Count > 0)
                {
                    foreach (var tagId in tagIds)
                    {
                        var gameTag = new GameTag
                        {
                            GameId = createdGame.Id,
                            TagId = tagId
                        };
                        await _gameTagRepository.CreateAsync(gameTag);
                    }
                }

                return RedirectToAction(nameof(Games));
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Categories = (await _categoryRepository.GetAllAsync()).ToList();
            ViewBag.Tags = (await _tagRepository.GetAllAsync()).ToList();
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

            var game = await _gameRepository.GetByIdAsync(id.Value);
            
            if (game == null)
            {
                return NotFound();
            }

            // Get categories for dropdown
            ViewBag.Categories = (await _categoryRepository.GetAllAsync()).ToList();
            // Get tags for multiselect
            ViewBag.Tags = (await _tagRepository.GetAllAsync()).ToList();
            // Get selected tag IDs
            var gameTags = await _gameTagRepository.GetByGameIdAsync(id.Value);
            ViewBag.SelectedTagIds = gameTags.Select(gt => gt.TagId).ToList();

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
                    var existingGame = await _gameRepository.GetByIdAsync(game.Id);
                    if (existingGame == null)
                    {
                        return NotFound();
                    }

                    // Lưu đường dẫn ảnh cũ trước để sau này gán lại nếu không có ảnh mới
                    string? existingCoverImageUrl = existingGame.CoverImageUrl;

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
                    
                    await _gameRepository.UpdateAsync(existingGame);
                    
                    // Update tags
                    // Remove all existing tags for the game
                    await _gameTagRepository.RemoveAllTagsFromGameAsync(game.Id);

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
                            await _gameTagRepository.CreateAsync(gameTag);
                        }
                    }

                    // Thêm thông báo thành công
                    TempData["SuccessMessage"] = "Game đã được cập nhật thành công!";
                    
                    return RedirectToAction(nameof(Games));
                }
                catch (Exception ex) when (!(ex is ArgumentException))
                {
                    if (!await _gameRepository.ExistsAsync(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        // Log lỗi và thêm vào ModelState để hiển thị
                        ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Categories = (await _categoryRepository.GetAllAsync()).ToList();
            ViewBag.Tags = (await _tagRepository.GetAllAsync()).ToList();
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

            var game = await _gameRepository.GetByIdAsync(id.Value);

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

            if (id == null)
            {
                return NotFound();
            }

            var game = await _gameRepository.GetByIdAsync(id.Value);
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

                // Delete related data using repository pattern
                try
                {
                    // Use bulk delete methods where available
                    await _screenshotRepository.DeleteScreenshotsByGameAsync(id.Value);
                    await _featureRepository.DeleteFeaturesByGameAsync(id.Value);
                    await _systemRequirementRepository.DeleteRequirementsByGameAsync(id.Value);
                    await _gameLinkRepository.DeleteLinksByGameAsync(id.Value);
                    await _gameTagRepository.RemoveAllTagsFromGameAsync(id.Value);
                    await _crackInfoRepository.DeleteCrackInfosByGameAsync(id.Value);
                    await _relatedGameRepository.RemoveAllRelatedGamesAsync(id.Value);

                    // For entities without bulk delete methods, get and delete individually
                    var reviews = await _reviewRepository.GetReviewsByGameAsync(id.Value);
                    foreach (var review in reviews)
                    {
                        await _reviewRepository.DeleteAsync(review.Id);
                    }

                    // For favorites, get all and filter by game ID since there's no direct GetByGameIdAsync method
                    var allFavorites = await _favoriteGameRepository.GetAllAsync();
                    var favoriteGames = allFavorites.Where(fg => fg.GameId == id.Value);
                    foreach (var favorite in favoriteGames)
                    {
                        // FavoriteGame has composite key (UserId, GameId), so we need to use both
                        await _favoriteGameRepository.DeleteAsync(favorite.UserId, favorite.GameId);
                    }

                    var downloadHistory = await _downloadHistoryRepository.GetDownloadsByGameAsync(id.Value);
                    foreach (var history in downloadHistory)
                    {
                        await _downloadHistoryRepository.DeleteAsync(history.Id);
                    }

                    var localizations = await _localizationInfoRepository.GetByGameIdAsync(id.Value);
                    foreach (var localization in localizations)
                    {
                        await _localizationInfoRepository.DeleteAsync(localization.Id);
                    }
                }
                catch (Exception)
                {
                    // Log the exception if needed
                    // For now, continue with game deletion
                }

                // Finally delete the game
                await _gameRepository.DeleteAsync(id.Value);
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

            var game = await _gameRepository.GetByIdAsync(id.Value);
                
            if (game == null)
            {
                return NotFound();
            }

            var screenshots = await _screenshotRepository.GetScreenshotsByGameAsync(id.Value);

            ViewBag.Game = game;
            return View(screenshots.ToList());
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

            var game = await _gameRepository.GetByIdAsync(gameId);
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

            await _screenshotRepository.CreateAsync(screenshot);

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

            var screenshot = await _screenshotRepository.GetByIdAsync(id);
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

            await _screenshotRepository.DeleteAsync(id);

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

            var game = await _gameRepository.GetByIdAsync(id.Value);
                
            if (game == null)
            {
                return NotFound();
            }

            var gameLinks = await _gameLinkRepository.GetLinksByGameAsync(id.Value);

            ViewBag.Game = game;
            return View(gameLinks.ToList());
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
                await _gameLinkRepository.CreateAsync(gameLink);
                return RedirectToAction(nameof(ManageLinks), new { id = gameLink.GameId });
            }

            var game = await _gameRepository.GetByIdAsync(gameLink.GameId);
            var gameLinks = await _gameLinkRepository.GetLinksByGameAsync(gameLink.GameId);
                
            ViewBag.Game = game;
            return View(nameof(ManageLinks), gameLinks.ToList());
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

            var gameLink = await _gameLinkRepository.GetByIdAsync(id);
            if (gameLink == null)
            {
                return NotFound();
            }

            var gameId = gameLink.GameId;

            await _gameLinkRepository.DeleteAsync(id);

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

            // Get all users with related data
            var allUsers = await _userRepository.GetAllAsync();

            // Apply search if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                allUsers = allUsers.Where(u => u.DisplayName.Contains(searchTerm) ||
                                                  (u.Email?.Contains(searchTerm) ?? false) ||
                                                  u.FirstName.Contains(searchTerm) ||
                                                  u.LastName.Contains(searchTerm)).ToList();
                ViewBag.SearchTerm = searchTerm;
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "name":
                    allUsers = allUsers.OrderBy(u => u.DisplayName).ToList();
                    break;
                case "name_desc":
                    allUsers = allUsers.OrderByDescending(u => u.DisplayName).ToList();
                    break;
                case "email":
                    allUsers = allUsers.OrderBy(u => u.Email).ToList();
                    break;
                case "email_desc":
                    allUsers = allUsers.OrderByDescending(u => u.Email).ToList();
                    break;
                case "role":
                    allUsers = allUsers.OrderBy(u => u.RoleId).ToList();
                    break;
                case "role_desc":
                    allUsers = allUsers.OrderByDescending(u => u.RoleId).ToList();
                    break;
                default: // newest
                    allUsers = allUsers.OrderByDescending(u => u.CreatedAt).ToList();
                    break;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.EmailSortParam = sortOrder == "email" ? "email_desc" : "email";
            ViewBag.RoleSortParam = sortOrder == "role" ? "role_desc" : "role";

            // Count total users for pagination
            var totalUsers = allUsers.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / PageSize);

            // Get users for current page
            var users = allUsers
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Roles = await _roleRepository.GetAllAsync();

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

            ViewBag.Roles = await _roleRepository.GetAllAsync();
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
                if (await _userRepository.GetByDisplayNameAsync(user.DisplayName) != null)
                {
                    ModelState.AddModelError("DisplayName", "Username already exists");
                    ViewBag.Roles = await _roleRepository.GetAllAsync();
                    return View(user);
                }

                // Check if email already exists
                if (!string.IsNullOrEmpty(user.Email) && await _userRepository.EmailExistsAsync(user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    ViewBag.Roles = await _roleRepository.GetAllAsync();
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
                await _userRepository.CreateAsync(user);

                return RedirectToAction(nameof(Users));
            }

            // If we got this far, something failed
            ViewBag.Roles = await _roleRepository.GetAllAsync();
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

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Roles = await _roleRepository.GetAllAsync();
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
                var existingUser = await _userRepository.GetByIdAsync(user.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                // Check username uniqueness if changed
                var existingUserByDisplayName = await _userRepository.GetByDisplayNameAsync(user.DisplayName);
                if (existingUser.DisplayName != user.DisplayName && 
                    existingUserByDisplayName != null && existingUserByDisplayName.Id != user.Id)
                {
                    ModelState.AddModelError("DisplayName", "Username already exists");
                    ViewBag.Roles = await _roleRepository.GetAllAsync();
                    return View(user);
                }

                // Check email uniqueness if changed
                if (!string.IsNullOrEmpty(user.Email) && existingUser.Email != user.Email && 
                    await _userRepository.EmailExistsAsync(user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    ViewBag.Roles = await _roleRepository.GetAllAsync();
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

                // Save changes to database
                await _userRepository.UpdateAsync(existingUser);
                
                TempData["SuccessMessage"] = "Thông tin người dùng đã được cập nhật thành công!";
                return RedirectToAction(nameof(Users));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _userRepository.ExistsAsync(user.Id))
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
            ViewBag.Roles = await _roleRepository.GetAllAsync();
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

            var user = await _userRepository.GetByIdAsync(id);
                
            if (user == null)
            {
                return NotFound();
            }

            // Kiểm tra nếu đây là tài khoản Admin duy nhất
            if (user.RoleId == 3)
            {
                var adminUsers = await _userRepository.GetUsersByRoleAsync(3);
                if (adminUsers.Count() <= 1)
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

            // Xóa dữ liệu liên quan qua repositories
            // Reviews - sử dụng repository method nếu có, hoặc fallback to context
            var userReviews = await _reviewRepository.GetReviewsByUserAsync(id);
            foreach (var review in userReviews)
            {
                await _reviewRepository.DeleteAsync(review.Id);
            }

            // Favorite games
            var userFavorites = await _favoriteGameRepository.GetFavoritesByUserAsync(id);
            foreach (var favorite in userFavorites)
            {
                await _favoriteGameRepository.DeleteAsync(favorite.UserId, favorite.GameId);
            }

            // Download history
            var userDownloadHistory = await _downloadHistoryRepository.GetDownloadsByUserAsync(id);
            foreach (var downloadRecord in userDownloadHistory)
            {
                await _downloadHistoryRepository.DeleteAsync(downloadRecord.Id);
            }

            // Search history
            var userSearchHistory = await _searchHistoryRepository.GetSearchesByUserAsync(id);
            foreach (var searchRecord in userSearchHistory)
            {
                await _searchHistoryRepository.DeleteAsync(searchRecord.Id);
            }

            // Xóa người dùng
            await _userRepository.DeleteAsync(id);

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

            var user = await _userRepository.GetByIdAsync(id);
                
            if (user == null)
            {
                return NotFound();
            }

            // Get user activity using repositories
            var recentDownloads = (await _downloadHistoryRepository.GetDownloadsByUserAsync(id))
                .OrderByDescending(d => d.DownloadDate)
                .Take(10)
                .ToList();

            var favoriteGames = (await _favoriteGameRepository.GetFavoritesByUserAsync(id))
                .OrderByDescending(f => f.DateAdded)
                .Take(10)
                .ToList();

            var reviews = (await _reviewRepository.GetReviewsByUserAsync(id))
                .OrderByDescending(r => r.DatePosted)
                .Take(10)
                .ToList();

            ViewBag.RecentDownloads = recentDownloads.ToList();
            ViewBag.FavoriteGames = favoriteGames.ToList();
            ViewBag.Reviews = reviews.ToList();

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

            var game = await _gameRepository.GetByIdAsync(id.Value);
            if (game == null)
            {
                return NotFound();
            }

            var systemRequirements = await _systemRequirementRepository.GetRequirementsByGameAsync(id.Value);

            ViewBag.Game = game ?? new object();
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
                    await _systemRequirementRepository.UpdateAsync(systemRequirement);
                    TempData["SuccessMessage"] = "Cập nhật cấu hình thành công!";
                }
                else
                {
                    // Create new requirement
                    // Check if a requirement of this type already exists for the game
                    var existingRequirements = await _systemRequirementRepository.GetRequirementsByGameAsync(systemRequirement.GameId);
                    var existingRequirement = existingRequirements.FirstOrDefault(sr => sr.RequirementType == systemRequirement.RequirementType);

                    if (existingRequirement != null)
                    {
                        // Update existing requirement
                        existingRequirement.OS = systemRequirement.OS;
                        existingRequirement.Processor = systemRequirement.Processor;
                        existingRequirement.Memory = systemRequirement.Memory;
                        existingRequirement.Graphics = systemRequirement.Graphics;
                        existingRequirement.DirectX = systemRequirement.DirectX;
                        existingRequirement.Storage = systemRequirement.Storage;
                        await _systemRequirementRepository.UpdateAsync(existingRequirement);
                    }
                    else
                    {
                        // Create new requirement
                        await _systemRequirementRepository.CreateAsync(systemRequirement);
                    }
                    TempData["SuccessMessage"] = "Thêm cấu hình thành công!";
                }

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

            var game = await _gameRepository.GetByIdAsync(id.Value);
            if (game == null)
            {
                return NotFound();
            }

            var crackInfos = await _crackInfoRepository.GetCrackInfosByGameAsync(id.Value);

            ViewBag.Game = game ?? new object();
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
                var existingCrackInfos = await _crackInfoRepository.GetCrackInfosByGameAsync(crackInfo.GameId);
                if (!existingCrackInfos.Any())
                {
                    crackInfo.IsRecommended = true;
                }
                else if (crackInfo.IsRecommended)
                {
                    // If this is marked as recommended, unmark others
                    var existingRecommended = existingCrackInfos.Where(ci => ci.IsRecommended).ToList();

                    foreach (var ci in existingRecommended)
                    {
                        ci.IsRecommended = false;
                        await _crackInfoRepository.UpdateAsync(ci);
                    }
                }

                await _crackInfoRepository.CreateAsync(crackInfo);

                TempData["SuccessMessage"] = "Thêm thông tin crack thành công!";
                return RedirectToAction(nameof(ManageCrackInfos), new { id = crackInfo.GameId });
            }

            TempData["ErrorMessage"] = "Có lỗi xảy ra, vui lòng kiểm tra lại dữ liệu nhập vào!";
            var game = await _gameRepository.GetByIdAsync(crackInfo.GameId);
            ViewBag.Game = game ?? new object();
            
            var crackInfos = await _crackInfoRepository.GetCrackInfosByGameAsync(crackInfo.GameId);
                
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
                    var existingCrackInfo = await _crackInfoRepository.GetByIdAsync(crackInfo.Id);
                    if (existingCrackInfo == null)
                    {
                        return NotFound();
                    }

                    // If this is marked as recommended, unmark others
                    if (crackInfo.IsRecommended && !existingCrackInfo.IsRecommended)
                    {
                        var existingCrackInfos = await _crackInfoRepository.GetCrackInfosByGameAsync(crackInfo.GameId);
                        var existingRecommended = existingCrackInfos.Where(ci => ci.IsRecommended).ToList();

                        foreach (var ci in existingRecommended)
                        {
                            ci.IsRecommended = false;
                            await _crackInfoRepository.UpdateAsync(ci);
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

                    await _crackInfoRepository.UpdateAsync(existingCrackInfo);

                    TempData["SuccessMessage"] = "Cập nhật thông tin crack thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _crackInfoRepository.ExistsAsync(crackInfo.Id))
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

            var crackInfo = await _crackInfoRepository.GetByIdAsync(id);
            if (crackInfo == null)
            {
                return NotFound();
            }

            var gameId = crackInfo.GameId;
            var wasRecommended = crackInfo.IsRecommended;

            await _crackInfoRepository.DeleteAsync(id);

            // If this was the recommended crack, set another one as recommended if any exist
            if (wasRecommended)
            {
                var crackInfos = await _crackInfoRepository.GetCrackInfosByGameAsync(gameId);
                var anotherCrack = crackInfos.FirstOrDefault();

                if (anotherCrack != null)
                {
                    anotherCrack.IsRecommended = true;
                    await _crackInfoRepository.UpdateAsync(anotherCrack);
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

            var crackInfo = await _crackInfoRepository.GetByIdAsync(id);
            if (crackInfo == null)
            {
                return NotFound();
            }

            // Get all crack infos for this game
            var allCrackInfos = await _crackInfoRepository.GetCrackInfosByGameAsync(crackInfo.GameId);
            
            // Unmark all other crack infos for this game
            var otherCracks = allCrackInfos.Where(ci => ci.Id != id && ci.IsRecommended).ToList();

            foreach (var crack in otherCracks)
            {
                crack.IsRecommended = false;
                await _crackInfoRepository.UpdateAsync(crack);
            }

            // Mark this one as recommended
            crackInfo.IsRecommended = true;
            await _crackInfoRepository.UpdateAsync(crackInfo);

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

            var game = await _gameRepository.GetByIdAsync(id.Value);
            if (game == null)
            {
                return NotFound();
            }

            var features = await _featureRepository.GetFeaturesByGameAsync(id.Value);

            ViewBag.Game = game ?? new object();
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
                await _featureRepository.CreateAsync(feature);

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
                    await _featureRepository.UpdateAsync(feature);
                    TempData["SuccessMessage"] = "Cập nhật tính năng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _featureRepository.ExistsAsync(feature.Id))
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

            var feature = await _featureRepository.GetByIdAsync(id);
            if (feature == null)
            {
                return NotFound();
            }

            var gameId = feature.GameId;

            await _featureRepository.DeleteAsync(id);

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

            // Get all categories
            var categories = (await _categoryRepository.GetAllAsync()).ToList();

            // Apply search if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                categories = categories.Where(c => c.CategoryName.Contains(searchTerm) ||
                                                   (c.Description?.Contains(searchTerm) ?? false)).ToList();
                ViewBag.SearchTerm = searchTerm;
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "name_desc":
                    categories = categories.OrderByDescending(c => c.CategoryName).ToList();
                    break;
                case "games":
                    categories = categories.OrderByDescending(c => c.GameCount).ToList();
                    break;
                case "games_asc":
                    categories = categories.OrderBy(c => c.GameCount).ToList();
                    break;
                default: // name
                    categories = categories.OrderBy(c => c.CategoryName).ToList();
                    break;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.GamesSortParam = sortOrder == "games" ? "games_asc" : "games";

            // Count total categories for pagination
            var totalCategories = categories.Count;
            var totalPages = (int)Math.Ceiling((double)totalCategories / PageSize);

            // Get categories for current page
            var pagedCategories = categories
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedCategories);
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
                if (await _categoryRepository.SlugExistsAsync(category.Slug))
                {
                    // Append a number to the slug to make it unique
                    var baseSlug = category.Slug;
                    var slugCount = 1;
                    while (await _categoryRepository.SlugExistsAsync(category.Slug))
                    {
                        category.Slug = $"{baseSlug}-{slugCount}";
                        slugCount++;
                    }
                }

                await _categoryRepository.CreateAsync(category);
                
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

            var category = await _categoryRepository.GetByIdAsync(id.Value);
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

                    // Check if a category with the same slug (but different ID) already exists
                    var categories = await _categoryRepository.GetAllAsync();
                    var existingCategory = categories.FirstOrDefault(c => c.Slug == category.Slug && c.CategoryId != category.CategoryId);

                    if (existingCategory != null)
                    {
                        // Append a number to the slug to make it unique
                        var baseSlug = category.Slug;
                        var slugCount = 1;
                        while (categories.Any(c => c.Slug == category.Slug && c.CategoryId != category.CategoryId))
                        {
                            category.Slug = $"{baseSlug}-{slugCount}";
                            slugCount++;
                        }
                    }

                    await _categoryRepository.UpdateAsync(category);
                    
                    TempData["SuccessMessage"] = "Thể loại đã được cập nhật thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CategoryExists(category.CategoryId))
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

            var category = await _categoryRepository.GetByIdAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }

            // Check if category has games
            var games = await _gameRepository.GetAllAsync();
            var gamesCount = games.Count(g => g.CategoryId == id);
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

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Check if category has games
            var games = await _gameRepository.GetAllAsync();
            var hasGames = games.Any(g => g.CategoryId == id);
            if (hasGames)
            {
                TempData["ErrorMessage"] = "Không thể xóa thể loại này vì có game thuộc thể loại này!";
                return RedirectToAction(nameof(Categories));
            }

            await _categoryRepository.DeleteAsync(id);

            TempData["SuccessMessage"] = "Thể loại đã được xóa thành công!";
            return RedirectToAction(nameof(Categories));
        }

        private async Task<bool> CategoryExists(int id)
        {
            return await _categoryRepository.ExistsAsync(id);
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

            // Get all tags
            var allTags = await _tagRepository.GetAllAsync();

            // Apply search if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                allTags = allTags.Where(t => t.Name.Contains(searchTerm) || 
                                           (t.Slug != null && t.Slug.Contains(searchTerm))).ToList();
                ViewBag.SearchTerm = searchTerm;
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "name_desc":
                    allTags = allTags.OrderByDescending(t => t.Name).ToList();
                    break;
                default: // name
                    allTags = allTags.OrderBy(t => t.Name).ToList();
                    break;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "name" ? "name_desc" : "name";

            // Count total tags for pagination
            var totalTags = allTags.Count();
            var totalPages = (int)Math.Ceiling((double)totalTags / PageSize);

            // Get tags for current page
            var tags = allTags
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            // Count games for each tag
            var gameTags = await _gameTagRepository.GetAllAsync();
            var tagIdCounts = gameTags
                .GroupBy(gt => gt.TagId)
                .ToDictionary(g => g.Key, g => g.Count());

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
                if (await _tagRepository.NameExistsAsync(tag.Name))
                {
                    ModelState.AddModelError("Name", "Tag với tên này đã tồn tại!");
                    return View(tag);
                }

                // Check if slug already exists
                var allTags = await _tagRepository.GetAllAsync();
                if (allTags.Any(t => t.Slug == tag.Slug))
                {
                    // Append a number to the slug to make it unique
                    var baseSlug = tag.Slug;
                    var slugCount = 1;
                    while (allTags.Any(t => t.Slug == tag.Slug))
                    {
                        tag.Slug = $"{baseSlug}-{slugCount}";
                        slugCount++;
                    }
                }

                await _tagRepository.CreateAsync(tag);
                
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

            var tag = await _tagRepository.GetByIdAsync(id.Value);
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

                    // Get all tags to check for duplicates
                    var allTags = await _tagRepository.GetAllAsync();
                    
                    // Check if name already exists for other tags
                    var existingNameTag = allTags.FirstOrDefault(t => t.Name == tag.Name && t.Id != tag.Id);

                    if (existingNameTag != null)
                    {
                        ModelState.AddModelError("Name", "Tag với tên này đã tồn tại!");
                        return View(tag);
                    }

                    // Check if slug already exists for other tags
                    var existingSlugTag = allTags.FirstOrDefault(t => t.Slug == tag.Slug && t.Id != tag.Id);

                    if (existingSlugTag != null)
                    {
                        // Append a number to the slug to make it unique
                        var baseSlug = tag.Slug;
                        var slugCount = 1;
                        while (allTags.Any(t => t.Slug == tag.Slug && t.Id != tag.Id))
                        {
                            tag.Slug = $"{baseSlug}-{slugCount}";
                            slugCount++;
                        }
                    }

                    await _tagRepository.UpdateAsync(tag);
                    
                    TempData["SuccessMessage"] = "Tag đã được cập nhật thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TagExists(tag.Id))
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

            var tag = await _tagRepository.GetByIdAsync(id.Value);
            if (tag == null)
            {
                return NotFound();
            }

            // Check if tag has games
            var gameTags = await _gameTagRepository.GetAllAsync();
            var gameCount = gameTags.Count(gt => gt.TagId == id);
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

            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            // Delete all game-tag associations using the repository
            await _gameTagRepository.RemoveGameFromAllTagsAsync(id);

            // Delete the tag using the repository
            await _tagRepository.DeleteAsync(id);

            TempData["SuccessMessage"] = "Tag đã được xóa thành công!";
            return RedirectToAction(nameof(Tags));
        }

        private async Task<bool> TagExists(int id)
        {
            return await _tagRepository.ExistsAsync(id);
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

        private async Task<bool> GameExists(int id)
        {
            return await _gameRepository.ExistsAsync(id);
        }

        private async Task<bool> UserExists(string id)
        {
            return await _userRepository.ExistsAsync(id);
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
        
        private async Task<bool> CrackInfoExists(int id)
        {
            return await _crackInfoRepository.ExistsAsync(id);
        }
        
        private async Task<bool> FeatureExists(int id)
        {
            return await _featureRepository.ExistsAsync(id);
        }
        
        #endregion
    }
}