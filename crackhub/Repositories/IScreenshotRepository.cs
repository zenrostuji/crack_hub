using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IScreenshotRepository
    {
        Task<IEnumerable<Screenshot>> GetAllAsync();
        Task<Screenshot?> GetByIdAsync(int id);
        Task<Screenshot> CreateAsync(Screenshot screenshot);
        Task<Screenshot> UpdateAsync(Screenshot screenshot);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Screenshot>> GetScreenshotsByGameAsync(int gameId);
        Task<int> GetScreenshotsCountByGameAsync(int gameId);
        Task<bool> DeleteScreenshotsByGameAsync(int gameId);
        Task<int> GetTotalScreenshotsCountAsync();
    }
}
