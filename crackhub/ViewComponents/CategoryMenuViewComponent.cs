using crackhub.Models.Data;
using crackhub.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace crackhub.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryMenuViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Get all categories with their game counts and order them by name
            var categories = await _categoryRepository.GetCategoriesWithGamesAsync();
            var orderedCategories = categories.OrderBy(c => c.CategoryName).ToList();

            return View(orderedCategories);
        }
    }
}