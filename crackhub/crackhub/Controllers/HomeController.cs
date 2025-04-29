using crackhub.Models;
using crackhub.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace crackhub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy 8 game mới nhất
            var latestGames = await _context.Games
                .Include(g => g.Category)
                .OrderByDescending(g => g.Id)
                .Take(8)
                .ToListAsync();

            ViewBag.LatestGames = latestGames;

            // Lấy 8 game phổ biến nhất (dựa trên số lượt tải)
            var popularGames = await _context.Games
                .Include(g => g.Category)
                .OrderByDescending(g => g.Downloads)
                .Take(8)
                .ToListAsync();

            ViewBag.PopularGames = popularGames;

            // Lấy 8 game có đánh giá cao nhất
            var topRatedGames = await _context.Games
                .Include(g => g.Category)
                .OrderByDescending(g => g.AverageRating)
                .Take(8)
                .ToListAsync();

            ViewBag.TopRatedGames = topRatedGames;

            // Lấy danh sách các thể loại
            var categories = await _context.Categories
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            return View(categories);
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
    }
}
