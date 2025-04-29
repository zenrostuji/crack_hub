using Microsoft.AspNetCore.Mvc;
using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace crackhub.Controllers
{
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int PageSize = 12; // Số game hiển thị trên mỗi trang

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Game
        public async Task<IActionResult> Index(int page = 1, string sort = "newest", string query = null)
        {
            IQueryable<Game> gamesQuery = _context.Games.Include(g => g.Category);
            
            // Apply search query if provided
            if (!string.IsNullOrEmpty(query))
            {
                gamesQuery = gamesQuery.Where(g => g.Title.Contains(query) || 
                                         g.ShortDescription.Contains(query) || 
                                         g.FullDescription.Contains(query));
                ViewBag.SearchQuery = query;
            }
            
            // Apply sorting
            switch (sort.ToLower())
            {
                case "popular":
                    gamesQuery = gamesQuery.OrderByDescending(g => g.Downloads);
                    break;
                case "rating":
                    gamesQuery = gamesQuery.OrderByDescending(g => g.AverageRating);
                    break;
                default: // newest
                    gamesQuery = gamesQuery.OrderByDescending(g => g.Id);
                    break;
            }
            
            var totalGames = await gamesQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalGames / PageSize);
            
            var games = await gamesQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            
            // Get user's favorite games for favorites button
            if (User.Identity.IsAuthenticated)
            {
                var userId = GetUserId();
                
                if (!string.IsNullOrEmpty(userId))
                {
                    var favoriteGameIds = await _context.FavoriteGames
                        .Where(fg => fg.UserId == userId)
                        .Select(fg => fg.GameId)
                        .ToListAsync();
                    
                    ViewBag.FavoriteGameIds = favoriteGameIds;
                }
            }
            
            // Get stats for hero section
            // Fix CS8198 error by calculating downloads differently
            var allGames = await _context.Games.ToListAsync();
            int totalDownloads = 0;
            foreach (var g in allGames)
            {
                if (int.TryParse(g.Downloads, out int downloads))
                {
                    totalDownloads += downloads;
                }
            }
            ViewBag.TotalDownloads = totalDownloads;
            ViewBag.TotalUsers = await _context.Users.CountAsync();

            return View(games);
        }

        // GET: /Game/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var game = await _context.Games
                .Include(g => g.Category)
                .Include(g => g.Screenshots)
                .Include(g => g.SystemRequirements)
                .Include(g => g.Features)
                .Include(g => g.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            try
            {
                // Try to include CrackInfos
                var gameCrackInfos = await _context.CrackInfos
                    .Where(c => c.GameId == id)
                    .ToListAsync();
                    
                game.CrackInfos = gameCrackInfos;
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
                var isFavorite = await _context.FavoriteGames
                    .AnyAsync(fg => fg.GameId == id && fg.UserId == userId);
                
                ViewBag.IsFavorite = isFavorite;
            }
            else
            {
                ViewBag.IsFavorite = false;
            }

            // Lấy danh sách game liên quan
            var relatedGames = await _context.RelatedGames
                .Where(rg => rg.GameId == id)
                .Include(rg => rg.RelatedTo)
                .Select(rg => rg.RelatedTo)
                .Take(4)
                .ToListAsync();

            ViewBag.RelatedGames = relatedGames;

            return View(game);
        }

        // GET: /Game/Download/5
        public async Task<IActionResult> Download(int id)
        {
            var game = await _context.Games
                .Include(g => g.Category)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            // Tăng số lượt download
            game.Downloads = (int.TryParse(game.Downloads, out int downloads) ? downloads + 1 : 1).ToString();
            await _context.SaveChangesAsync();

            // Lấy các link tải
            var gameLinks = await _context.GameLinks
                .Where(gl => gl.GameId == id && gl.IsActive)
                .ToListAsync();

            // Initialize an empty collection for crack links
            IList<CrackInfo> crackLinks = new List<CrackInfo>();
            
            try
            {
                // Try to get crack links
                crackLinks = await _context.CrackInfos
                    .Where(c => c.GameId == id)
                    .ToListAsync();
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
                DownloadDate = DateTime.Now,
                IP = HttpContext.Connection.RemoteIpAddress?.ToString()
            };

            _context.DownloadHistory.Add(downloadHistory);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // GET: /Game/ByCategory/{categorySlug}
        public async Task<IActionResult> ByCategory(string categorySlug, int page = 1)
        {
            if (string.IsNullOrEmpty(categorySlug))
            {
                return RedirectToAction("Index");
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Slug == categorySlug);

            if (category == null)
            {
                return NotFound();
            }

            // Get total games in this category
            var totalGames = await _context.Games
                .Where(g => g.CategoryId == category.CategoryId)
                .CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalGames / PageSize);

            // Get games for current page
            var games = await _context.Games
                .Where(g => g.CategoryId == category.CategoryId)
                .Include(g => g.Category)
                .OrderByDescending(g => g.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Category = category;
            ViewBag.TotalGameCount = totalGames;

            // Calculate statistics for the category
            ViewBag.TopRatedGame = await _context.Games
                .Where(g => g.CategoryId == category.CategoryId)
                .OrderByDescending(g => g.AverageRating)
                .FirstOrDefaultAsync();

            ViewBag.MostDownloaded = await _context.Games
                .Where(g => g.CategoryId == category.CategoryId)
                .OrderByDescending(g => g.Downloads)
                .FirstOrDefaultAsync();

            return View(games);
        }

        // GET: /Game/Category/5
        public async Task<IActionResult> Category(int id, int page = 1)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var totalGames = await _context.Games
                .Where(g => g.CategoryId == id)
                .CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalGames / PageSize);

            var games = await _context.Games
                .Where(g => g.CategoryId == id)
                .Include(g => g.Category)
                .OrderByDescending(g => g.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Category = category;

            return View(games);
        }

        // GET: /Game/Search
        public async Task<IActionResult> Search(string query, int page = 1)
        {
            if (string.IsNullOrEmpty(query))
            {
                return RedirectToAction("Index");
            }

            var totalGames = await _context.Games
                .Where(g => g.Title.Contains(query) || 
                            g.ShortDescription.Contains(query) || 
                            g.FullDescription.Contains(query))
                .CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalGames / PageSize);

            var games = await _context.Games
                .Where(g => g.Title.Contains(query) || 
                            g.ShortDescription.Contains(query) || 
                            g.FullDescription.Contains(query))
                .Include(g => g.Category)
                .OrderByDescending(g => g.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = query;

            return View("Index", games);
        }

        // POST: /Game/AddReview
        [HttpPost]
        public async Task<IActionResult> AddReview(int gameId, string title, string content, int rating)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để đánh giá game!" });
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content) || rating < 1 || rating > 5)
            {
                return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin đánh giá!" });
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var review = new Review
            {
                GameId = gameId,
                UserId = userId,
                Title = title,
                Content = content,
                Rating = rating,
                DatePosted = DateTime.Now
            };

            _context.Reviews.Add(review);

            // Cập nhật đánh giá trung bình của game
            var game = await _context.Games.FindAsync(gameId);
            if (game != null)
            {
                var allRatings = await _context.Reviews
                    .Where(r => r.GameId == gameId)
                    .Select(r => r.Rating)
                    .ToListAsync();

                allRatings.Add(rating);
                game.AverageRating = allRatings.Average();
                _context.Update(game);
            }

            await _context.SaveChangesAsync();

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
            var game = await _context.Games.FindAsync(gameId);
            if (game == null)
            {
                return Json(new { success = false, message = "Không tìm thấy game!" });
            }

            var existingFavorite = await _context.FavoriteGames
                .FirstOrDefaultAsync(fg => fg.GameId == gameId && fg.UserId == userId);

            bool isFavorite;

            if (existingFavorite != null)
            {
                // Đã yêu thích, xóa khỏi danh sách
                _context.FavoriteGames.Remove(existingFavorite);
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
                _context.FavoriteGames.Add(favoriteGame);
                isFavorite = true;
            }

            await _context.SaveChangesAsync();

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
            
            var favorite = await _context.FavoriteGames
                .FirstOrDefaultAsync(fg => fg.GameId == id && fg.UserId == userId);

            if (favorite == null)
            {
                return Json(new { success = false, message = "Không tìm thấy game trong danh sách yêu thích!" });
            }

            _context.FavoriteGames.Remove(favorite);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã xóa game khỏi danh sách yêu thích!" });
        }

        // Helper method to get current user ID
        private string GetUserId()
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
    }
}