using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IPremiumRepository
    {
        Task<IEnumerable<Premium>> GetAllAsync();
        Task<Premium?> GetByIdAsync(int id);
        Task<Premium> AddAsync(Premium premium);
        Task<Premium> UpdateAsync(Premium premium);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
