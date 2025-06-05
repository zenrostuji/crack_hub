using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> GetBySlugAsync(string slug);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> SlugExistsAsync(string slug);
        Task<IEnumerable<Category>> GetCategoriesWithGamesAsync();
        Task UpdateGameCountAsync(int categoryId);
        Task<int> GetTotalCategoriesCountAsync();
    }
}
