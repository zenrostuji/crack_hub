using Microsoft.AspNetCore.Mvc;
using crackhub.Models.Data;
using crackhub.Repositories;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace crackhub.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameRepository _gameRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFavoriteGameRepository _favoriteGameRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDownloadHistoryRepository _downloadHistoryRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ICrackInfoRepository _crackInfoRepository;
        private readonly IRelatedGameRepository _relatedGameRepository;
        private readonly IGameLinkRepository _gameLinkRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IGameTagRepository _gameTagRepository;
        private readonly int PageSize = 12; // Số game hiển thị trên mỗi trang

        public GameController(
            IGameRepository gameRepository,
            ICategoryRepository categoryRepository,
            IFavoriteGameRepository favoriteGameRepository,
            IUserRepository userRepository,
            IDownloadHistoryRepository downloadHistoryRepository,
            IReviewRepository reviewRepository,
            ICrackInfoRepository crackInfoRepository,
            IRelatedGameRepository relatedGameRepository,
            IGameLinkRepository gameLinkRepository,
            ITagRepository tagRepository,
            IGameTagRepository gameTagRepository)
        {
            _gameRepository = gameRepository;
            _categoryRepository = categoryRepository;
            _favoriteGameRepository = favoriteGameRepository;
            _userRepository = userRepository;
            _downloadHistoryRepository = downloadHistoryRepository;
            _reviewRepository = reviewRepository;
            _crackInfoRepository = crackInfoRepository;
            _relatedGameRepository = relatedGameRepository;
            _gameLinkRepository = gameLinkRepository;
            _tagRepository = tagRepository;
            _gameTagRepository = gameTagRepository;
        }        // GET: /Game
        public async Task<IActionResult> Index(int page = 1, string sort = "newest", string? query = null)
        {
            IEnumerable<Game> games;
            
            // Apply search query if provided
            if (!string.IsNullOrEmpty(query))
            {
                games = await _gameRepository.SearchGamesAsync(query);
                ViewBag.SearchQuery = query;
            }
            else
            {
                games = await _gameRepository.GetAllAsync();
            }
            
            // Apply sorting
            switch (sort.ToLower())
            {
                case "popular":
                    games = games.OrderByDescending(g => int.TryParse(g.Downloads, out int downloads) ? downloads : 0);
                    break;
                case "rating":
                    games = games.OrderByDescending(g => g.AverageRating);
                    break;
                default: // newest
                    games = games.OrderByDescending(g => g.Id);
                    break;
            }
            
            var totalGames = games.Count();
            var totalPages = (int)Math.Ceiling((double)totalGames / PageSize);
            
            var pagedGames = games
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            
            // Get user's favorite games for favorites button
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = GetUserId();
                
                if (!string.IsNullOrEmpty(userId))
                {
                    var favoriteGames = await _favoriteGameRepository.GetFavoritesByUserAsync(userId);
                    var favoriteGameIds = favoriteGames.Select(fg => fg.GameId).ToList();
                    ViewBag.FavoriteGameIds = favoriteGameIds;
                }
            }
              // Get stats for hero section using efficient repository methods
            var totalDownloads = await _downloadHistoryRepository.GetTotalDownloadsCountAsync();
            ViewBag.TotalDownloads = totalDownloads;
            var totalUsers = await _userRepository.GetTotalUsersCountAsync();
            ViewBag.TotalUsers = totalUsers;

            return View(pagedGames);
        }

        // GET: /Game/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);

            if (game == null)
            {
                return NotFound();
            }            try
            {
                // Try to include CrackInfos
                var gameCrackInfos = await _crackInfoRepository.GetCrackInfosByGameAsync(id);
                game.CrackInfos = gameCrackInfos.ToList();
            }
            catch (Exception)
            {
                // If CrackInfos table doesn't exist, initialize an empty collection
                game.CrackInfos = new List<CrackInfo>();
            }

            // Check if the game is in the user's favorites - use session directly
            var userId = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userId))
            {
                var isFavorite = await _favoriteGameRepository.IsFavoriteAsync(userId, id);
                ViewBag.IsFavorite = isFavorite;
            }
            else
            {
                ViewBag.IsFavorite = false;
            }

            // Lấy danh sách game liên quan
            var relatedGames = await _relatedGameRepository.GetRelatedGamesByGameIdAsync(id);
            ViewBag.RelatedGames = relatedGames.Take(4).ToList();

            return View(game);
        }

        // GET: /Game/Download/5
        public async Task<IActionResult> Download(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            // Tăng số lượt download
            game.Downloads = (int.TryParse(game.Downloads, out int downloads) ? downloads + 1 : 1).ToString();
            await _gameRepository.UpdateAsync(game);

            // Lấy các link tải
            var gameLinks = await _gameLinkRepository.GetLinksByGameAsync(id);

            // Initialize an empty collection for crack links
            IEnumerable<CrackInfo> crackLinks = new List<CrackInfo>();
              try
            {
                // Try to get crack links
                crackLinks = await _crackInfoRepository.GetCrackInfosByGameAsync(id);
            }
            catch (Exception)
            {
                // If CrackInfos table doesn't exist, leave the collection empty
            }

            ViewBag.Game = game;
            ViewBag.CrackLinks = crackLinks;

            return View(gameLinks);
        }

        // POST: /Game/AddDownloadHistory
        [HttpPost]
        public async Task<IActionResult> AddDownloadHistory(int gameId)
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                // If not logged in, just return success (anonymous download)
                return Json(new { success = true });
            }

            // Record download history for logged in users
            var downloadHistory = new DownloadHistory
            {
                UserId = userId,
                GameId = gameId,
                DownloadDate = DateTime.Now
            };

            await _downloadHistoryRepository.CreateAsync(downloadHistory);

            return Json(new { success = true });
        }

        // GET: /Game/ByCategory/{categorySlug}
        public async Task<IActionResult> ByCategory(string categorySlug, int page = 1)
        {
            if (string.IsNullOrEmpty(categorySlug))
            {
                return RedirectToAction("Index");
            }

            var category = await _categoryRepository.GetBySlugAsync(categorySlug);

            if (category == null)
            {
                return NotFound();
            }

            // Get games for this category
            var allGames = await _gameRepository.GetGamesByCategoryAsync(category.CategoryId);
            var totalGames = allGames.Count();
            var totalPages = (int)Math.Ceiling((double)totalGames / PageSize);

            // Get games for current page
            var games = allGames
                .OrderByDescending(g => g.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Category = category;
            ViewBag.TotalGameCount = totalGames;

            // Calculate statistics for the category
            var topRatedGame = allGames.OrderByDescending(g => g.AverageRating).FirstOrDefault();
            var mostDownloaded = allGames.OrderByDescending(g => int.TryParse(g.Downloads, out int downloads) ? downloads : 0).FirstOrDefault();

            ViewBag.TopRatedGame = topRatedGame;
            ViewBag.MostDownloaded = mostDownloaded;

            return View(games);
        }

        // GET: /Game/Category/5
        public async Task<IActionResult> Category(int id, int page = 1)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var allGames = await _gameRepository.GetGamesByCategoryAsync(id);
            var totalGames = allGames.Count();
            var totalPages = (int)Math.Ceiling((double)totalGames / PageSize);

            var games = allGames
                .OrderByDescending(g => g.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Category = category;

            return View(games);
        }        // GET: /Game/Search
        public async Task<IActionResult> Search(string query, int page = 1)
        {
            if (string.IsNullOrEmpty(query))
            {
                return RedirectToAction("Index");
            }

            // Get total count efficiently
            var totalGames = await _gameRepository.GetSearchResultsCountAsync(query);
            var totalPages = (int)Math.Ceiling((double)totalGames / PageSize);

            // Get paginated results
            var allGames = await _gameRepository.SearchGamesAsync(query);
            var games = allGames
                .OrderByDescending(g => g.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = query;

            return View("Index", games);
        }

        // POST: /Game/AddReview
        [HttpPost]
        public async Task<IActionResult> AddReview(int gameId, string title, string content, int rating)
        {
            // Get user ID from session which is more consistent with the rest of the application
            var userId = HttpContext.Session.GetString("UserId");
            
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để đánh giá game!" });
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content) || rating < 1 || rating > 10)
            {
                return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin đánh giá!" });
            }

            var review = new Review
            {
                GameId = gameId,
                UserId = userId,
                Title = title,
                Content = content,
                Rating = rating,
                DatePosted = DateTime.Now
            };            await _reviewRepository.CreateAsync(review);
            
            // Update the game's average rating using repository method
            var newAverageRating = await _reviewRepository.GetAverageRatingByGameAsync(gameId);
            await _gameRepository.UpdateRatingAsync(gameId, newAverageRating);

            return Json(new { success = true, message = "Đánh giá của bạn đã được gửi thành công!" });
        }

        // POST: /Game/ToggleFavorite
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int gameId)
        {
            // Check if user is logged in using session directly since User.Identity.IsAuthenticated 
            // might not be correctly set in some environments
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thêm game vào danh sách yêu thích!" });
            }

            // Check if the game exists
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
            {
                return Json(new { success = false, message = "Không tìm thấy game!" });
            }

            var existingFavorite = await _favoriteGameRepository.GetFavoriteByUserAndGameAsync(userId, gameId);

            bool isFavorite;

            if (existingFavorite != null)
            {
                // Đã yêu thích, xóa khỏi danh sách
                await _favoriteGameRepository.DeleteAsync(userId, gameId);
                isFavorite = false;
            }
            else
            {
                // Chưa yêu thích, thêm vào danh sách
                var favoriteGame = new FavoriteGame
                {
                    UserId = userId,
                    GameId = gameId,
                    DateAdded = DateTime.Now
                };
                await _favoriteGameRepository.CreateAsync(favoriteGame);
                isFavorite = true;
            }

            return Json(new
            {
                success = true,
                isFavorite,
                message = isFavorite ? "Đã thêm vào danh sách yêu thích!" : "Đã xóa khỏi danh sách yêu thích!"
            });
        }
        
        // POST: /Game/RemoveFavorite
        [HttpPost]
        public async Task<IActionResult> RemoveFavorite(int id)
        {
            // Check if user is logged in using session directly
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thực hiện thao tác này!" });
            }
            
            var favorite = await _favoriteGameRepository.GetFavoriteByUserAndGameAsync(userId, id);

            if (favorite == null)
            {
                return Json(new { success = false, message = "Không tìm thấy game trong danh sách yêu thích!" });
            }

            await _favoriteGameRepository.DeleteAsync(userId, id);

            return Json(new { success = true, message = "Đã xóa game khỏi danh sách yêu thích!" });
        }

        // Helper method to get current user ID
        private string? GetUserId()
        {
            // Try to get from session first
            var userId = HttpContext.Session.GetString("UserId");
            
            if (string.IsNullOrEmpty(userId))
            {
                // Fallback to claims if session is empty
                userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                
                // Also update the session for consistency
                if (!string.IsNullOrEmpty(userId))
                {
                    HttpContext.Session.SetString("UserId", userId);
                }
            }
            
            return userId;
        }

        // GET: /Game/Tag/{tagSlug}
        public async Task<IActionResult> Tag(string id, int page = 1, string sort = "newest")
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }            // Tìm tag theo id hoặc theo tên
            Tag? tag = null;
            if (int.TryParse(id, out int tagId))
            {
                tag = await _tagRepository.GetByIdAsync(tagId);
            }
            else
            {
                tag = await _tagRepository.GetByNameAsync(id);
            }

            if (tag == null)
            {
                return NotFound();
            }            // Lấy tất cả game có tag này
            var allGames = await _gameRepository.GetGamesByTagAsync(tag.Id);

            // Áp dụng sắp xếp
            switch (sort.ToLower())
            {
                case "popular":
                    allGames = allGames.OrderByDescending(g => int.TryParse(g.Downloads, out int downloads) ? downloads : 0);
                    break;
                case "rating":
                    allGames = allGames.OrderByDescending(g => g.AverageRating);
                    break;
                default: // newest
                    allGames = allGames.OrderByDescending(g => g.Id);
                    break;
            }

            // Get total count efficiently and calculate pagination
            var totalGames = await _gameRepository.GetGamesCountByTagAsync(tag.Id);
            var totalPages = (int)Math.Ceiling((double)totalGames / PageSize);

            // Lấy game cho trang hiện tại
            var games = allGames
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            // Lấy tất cả các tag để hiển thị filter
            var allTags = await _tagRepository.GetAllAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Tag = tag;
            ViewBag.AllTags = allTags;
            ViewBag.CurrentSort = sort;
            ViewBag.TotalGameCount = totalGames;
            ViewBag.TagIds = new List<int> { tag.Id };

            // Get user's favorite games for favorites button
            var userId = GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                var favoriteGames = await _favoriteGameRepository.GetFavoritesByUserAsync(userId);
                var favoriteGameIds = favoriteGames.Select(fg => fg.GameId).ToList();
                ViewBag.FavoriteGameIds = favoriteGameIds;
            }

            return View("ByTag", games);
        }

        // GET: /Game/Tags - Lọc game theo nhiều tag
        public async Task<IActionResult> Tags(List<int> tags, int page = 1, string sort = "newest")
        {
            if (tags == null || !tags.Any())
            {
                return RedirectToAction("Index");
            }

            // Lấy thông tin của các tag đã chọn
            var selectedTags = new List<Tag>();
            foreach (var tagId in tags)
            {
                var tag = await _tagRepository.GetByIdAsync(tagId);
                if (tag != null)
                {
                    selectedTags.Add(tag);
                }
            }

            if (!selectedTags.Any())
            {
                return RedirectToAction("Index");
            }            // Lấy tất cả game có chứa TẤT CẢ các tag đã chọn sử dụng repository method hiệu quả
            var allGamesWithTags = await _gameRepository.GetGamesByMultipleTagsAsync(tags);
            
            // Áp dụng sắp xếp
            switch (sort.ToLower())
            {
                case "popular":
                    allGamesWithTags = allGamesWithTags.OrderByDescending(g => int.TryParse(g.Downloads, out int downloads) ? downloads : 0);
                    break;
                case "rating":
                    allGamesWithTags = allGamesWithTags.OrderByDescending(g => g.AverageRating);
                    break;
                default: // newest
                    allGamesWithTags = allGamesWithTags.OrderByDescending(g => g.Id);
                    break;
            }

            // Get total count efficiently
            var totalGames = await _gameRepository.GetGamesCountByMultipleTagsAsync(tags);
            var totalPages = (int)Math.Ceiling((double)totalGames / PageSize);

            // Lấy game cho trang hiện tại
            var games = allGamesWithTags
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            // Lấy tất cả các tag để hiển thị filter
            var allTags = await _tagRepository.GetAllAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SelectedTags = selectedTags;
            ViewBag.AllTags = allTags;
            ViewBag.CurrentSort = sort;
            ViewBag.TotalGameCount = totalGames;
            ViewBag.TagIds = tags;

            // Get user's favorite games for favorites button
            var userId = GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                var favoriteGames = await _favoriteGameRepository.GetFavoritesByUserAsync(userId);
                var favoriteGameIds = favoriteGames.Select(fg => fg.GameId).ToList();
                ViewBag.FavoriteGameIds = favoriteGameIds;
            }

            return View("ByTag", games);
        }
    }
}
