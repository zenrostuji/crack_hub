using crackhub.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace crackhub.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoryMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Get all categories with their game counts and order them by name
            var categories = await _context.Categories
                .Include(c => c.Games)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            return View(categories);
        }
    }
}