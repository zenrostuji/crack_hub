using crackhub.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Controllers
{
    public class ModeratorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int PageSize = 10; // Số lượng tag hiển thị trên mỗi trang

        public ModeratorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Dashboard
        public async Task<IActionResult> Index()
        {
            // Kiểm tra quyền truy cập
            if (!IsModerator())
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy các thông số thống kê cho dashboard
            ViewBag.TotalTags = await _context.Tags.CountAsync();
            ViewBag.TotalGames = await _context.Games.CountAsync();
            
            // Lấy danh sách tags mới nhất
            var recentTags = await _context.Tags
                .OrderByDescending(t => t.Id)
                .Take(5)
                .ToListAsync();
            ViewBag.RecentTags = recentTags;

            // Đếm số lượng game theo tag
            var tagCounts = await _context.GameTags
                .GroupBy(gt => gt.TagId)
                .Select(g => new { TagId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.TagId, g => g.Count);
            ViewBag.TagCounts = tagCounts;

            return View();
        }

        #region Tag Management

        // Danh sách các tags với phân trang
        public async Task<IActionResult> Tags(int page = 1, string searchTerm = "", string sortOrder = "name")
        {
            // Kiểm tra quyền truy cập
            if (!IsModerator())
            {
                return RedirectToAction("Login", "Account");
            }

            // Tạo query
            IQueryable<Tag> tagsQuery = _context.Tags;

            // Áp dụng tìm kiếm nếu có
            if (!string.IsNullOrEmpty(searchTerm))
            {
                tagsQuery = tagsQuery.Where(t => t.Name.Contains(searchTerm) ||
                                               t.Slug.Contains(searchTerm));
                ViewBag.SearchTerm = searchTerm;
            }

            // Áp dụng sắp xếp
            switch (sortOrder)
            {
                case "name_desc":
                    tagsQuery = tagsQuery.OrderByDescending(t => t.Name);
                    break;
                case "id":
                    tagsQuery = tagsQuery.OrderBy(t => t.Id);
                    break;
                case "id_desc":
                    tagsQuery = tagsQuery.OrderByDescending(t => t.Id);
                    break;
                default: // name
                    tagsQuery = tagsQuery.OrderBy(t => t.Name);
                    break;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.IdSortParam = sortOrder == "id" ? "id_desc" : "id";

            // Đếm tổng số tags để phân trang
            var totalTags = await tagsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalTags / PageSize);

            // Lấy tags cho trang hiện tại
            var tags = await tagsQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            // Lấy số lượng game cho mỗi tag
            var tagGameCounts = await _context.GameTags
                .GroupBy(gt => gt.TagId)
                .Select(g => new { TagId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.TagId, g => g.Count);
            ViewBag.TagGameCounts = tagGameCounts;

            return View(tags);
        }

        // GET: Hiển thị form tạo tag
        public IActionResult CreateTag()
        {
            // Kiểm tra quyền truy cập
            if (!IsModerator())
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Tạo tag mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTag(Tag tag)
        {
            // Kiểm tra quyền truy cập
            if (!IsModerator())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Tạo slug nếu không được cung cấp
                if (string.IsNullOrEmpty(tag.Slug))
                {
                    tag.Slug = tag.Name.ToLower().Replace(" ", "-");
                }

                // Kiểm tra xem slug đã tồn tại chưa
                if (await _context.Tags.AnyAsync(t => t.Slug == tag.Slug))
                {
                    // Thêm số vào slug để tạo slug duy nhất
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

        // GET: Hiển thị form chỉnh sửa tag
        public async Task<IActionResult> EditTag(int? id)
        {
            // Kiểm tra quyền truy cập
            if (!IsModerator())
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

            // Lấy số lượng game cho tag này
            var gameCount = await _context.GameTags.CountAsync(gt => gt.TagId == id);
            ViewBag.GameCount = gameCount;

            return View(tag);
        }

        // POST: Cập nhật tag
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTag(Tag tag)
        {
            // Kiểm tra quyền truy cập
            if (!IsModerator())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Tạo slug nếu không được cung cấp
                    if (string.IsNullOrEmpty(tag.Slug))
                    {
                        tag.Slug = tag.Name.ToLower().Replace(" ", "-");
                    }

                    // Kiểm tra xem slug đã tồn tại cho các tag khác chưa
                    var existingTag = await _context.Tags
                        .FirstOrDefaultAsync(t => t.Slug == tag.Slug && t.Id != tag.Id);

                    if (existingTag != null)
                    {
                        // Thêm số vào slug để tạo slug duy nhất
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

        // GET: Xác nhận xóa tag
        public async Task<IActionResult> DeleteTag(int? id)
        {
            // Kiểm tra quyền truy cập
            if (!IsModerator())
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

            // Kiểm tra xem tag có gắn với game nào không
            var gamesCount = await _context.GameTags.CountAsync(gt => gt.TagId == id);
            ViewBag.HasGames = gamesCount > 0;
            ViewBag.GameCount = gamesCount;

            return View(tag);
        }

        // POST: Xóa tag
        [HttpPost, ActionName("DeleteTag")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTagConfirmed(int id)
        {
            // Kiểm tra quyền truy cập
            if (!IsModerator())
            {
                return RedirectToAction("Login", "Account");
            }

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            // Xóa tất cả các liên kết giữa tag này và các game
            var gameTags = await _context.GameTags.Where(gt => gt.TagId == id).ToListAsync();
            _context.GameTags.RemoveRange(gameTags);

            // Xóa tag
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Tag đã được xóa thành công!";
            return RedirectToAction(nameof(Tags));
        }

        #endregion

        #region Game Tag Management

        // Danh sách game để gán tag
        public async Task<IActionResult> GameTags(int page = 1, string searchTerm = "", string sortOrder = "newest")
        {
            // Kiểm tra quyền truy cập
            if (!IsModerator())
            {
                return RedirectToAction("Login", "Account");
            }

            // Tạo query
            IQueryable<Game> gamesQuery = _context.Games
                .Include(g => g.Category)
                .Include(g => g.GameTags)
                    .ThenInclude(gt => gt.Tag);

            // Áp dụng tìm kiếm nếu có
            if (!string.IsNullOrEmpty(searchTerm))
            {
                gamesQuery = gamesQuery.Where(g => g.Title.Contains(searchTerm) ||
                                                  g.ShortDescription.Contains(searchTerm) ||
                                                  g.Developer.Contains(searchTerm) ||
                                                  g.Publisher.Contains(searchTerm));
                ViewBag.SearchTerm = searchTerm;
            }

            // Áp dụng sắp xếp
            switch (sortOrder)
            {
                case "title":
                    gamesQuery = gamesQuery.OrderBy(g => g.Title);
                    break;
                case "title_desc":
                    gamesQuery = gamesQuery.OrderByDescending(g => g.Title);
                    break;
                default: // newest
                    gamesQuery = gamesQuery.OrderByDescending(g => g.Id);
                    break;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParam = sortOrder == "title" ? "title_desc" : "title";

            // Đếm tổng số game để phân trang
            var totalGames = await gamesQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalGames / PageSize);

            // Lấy games cho trang hiện tại
            var games = await gamesQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(games);
        }

        // GET: Hiển thị form quản lý tag cho game cụ thể
        public async Task<IActionResult> ManageGameTags(int? id)
        {
            // Kiểm tra quyền truy cập
            if (!IsModerator())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.GameTags)
                    .ThenInclude(gt => gt.Tag)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            // Lấy tất cả các tag
            var allTags = await _context.Tags.OrderBy(t => t.Name).ToListAsync();
            
            // Lấy ID của các tag đã được gán cho game
            var selectedTagIds = game.GameTags.Select(gt => gt.TagId).ToList();

            ViewBag.Game = game;
            ViewBag.AllTags = allTags;
            ViewBag.SelectedTagIds = selectedTagIds;

            return View(game);
        }

        // POST: Cập nhật tag cho game
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateGameTags(int gameId, List<int> tagIds)
        {
            // Kiểm tra quyền truy cập
            if (!IsModerator())
            {
                return RedirectToAction("Login", "Account");
            }

            var game = await _context.Games.FindAsync(gameId);
            if (game == null)
            {
                return NotFound();
            }

            // Xóa tất cả các liên kết tag hiện tại
            var existingTags = await _context.GameTags
                .Where(gt => gt.GameId == gameId)
                .ToListAsync();
            _context.GameTags.RemoveRange(existingTags);

            // Thêm các tag mới
            if (tagIds != null && tagIds.Count > 0)
            {
                foreach (var tagId in tagIds)
                {
                    var gameTag = new GameTag
                    {
                        GameId = gameId,
                        TagId = tagId
                    };
                    _context.GameTags.Add(gameTag);
                }
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Tag cho game đã được cập nhật thành công!";

            return RedirectToAction(nameof(GameTags));
        }

        #endregion

        #region Helper Methods

        private bool IsModerator()
        {
            // Kiểm tra xem người dùng hiện tại có quyền moderator hay không
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            var userRoleString = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(userRoleString) || !int.TryParse(userRoleString, out int userRole))
            {
                return false;
            }
            
            // Role 2 là Moderator, Role 3 là Admin (Admin cũng có thể truy cập các trang của Moderator)
            return userRole == 2 || userRole == 3;
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(t => t.Id == id);
        }

        #endregion
    }
}