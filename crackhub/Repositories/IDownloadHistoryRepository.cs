using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IDownloadHistoryRepository
    {
        Task<IEnumerable<DownloadHistory>> GetAllAsync();
        Task<DownloadHistory?> GetByIdAsync(int id);
        Task<DownloadHistory> CreateAsync(DownloadHistory downloadHistory);
        Task<DownloadHistory> UpdateAsync(DownloadHistory downloadHistory);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<DownloadHistory>> GetDownloadsByUserAsync(string userId);
        Task<IEnumerable<DownloadHistory>> GetDownloadsByGameAsync(int gameId);
        Task<DownloadHistory?> GetDownloadByUserAndGameAsync(string userId, int gameId);
        Task<int> GetDownloadsCountByUserAsync(string userId);
        Task<int> GetDownloadsCountByGameAsync(int gameId);
        Task<IEnumerable<DownloadHistory>> GetRecentDownloadsAsync(int count);
        Task<int> GetTotalDownloadsCountAsync();
        Task<IEnumerable<DownloadHistory>> GetDownloadsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
