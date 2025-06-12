using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFDownloadHistoryRepository : IDownloadHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public EFDownloadHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DownloadHistory>> GetAllAsync()
        {
            return await _context.DownloadHistory
                .Include(dh => dh.User)
                .Include(dh => dh.Game)
                .ToListAsync();
        }

        public async Task<DownloadHistory?> GetByIdAsync(int id)
        {
            return await _context.DownloadHistory
                .Include(dh => dh.User)
                .Include(dh => dh.Game)
                .FirstOrDefaultAsync(dh => dh.Id == id);
        }

        public async Task<DownloadHistory> CreateAsync(DownloadHistory downloadHistory)
        {
            _context.DownloadHistory.Add(downloadHistory);
            await _context.SaveChangesAsync();
            return downloadHistory;
        }

        public async Task<DownloadHistory> UpdateAsync(DownloadHistory downloadHistory)
        {
            _context.DownloadHistory.Update(downloadHistory);
            await _context.SaveChangesAsync();
            return downloadHistory;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var downloadHistory = await _context.DownloadHistory.FindAsync(id);
            if (downloadHistory == null) return false;

            _context.DownloadHistory.Remove(downloadHistory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.DownloadHistory.AnyAsync(dh => dh.Id == id);
        }

        public async Task<IEnumerable<DownloadHistory>> GetDownloadsByUserAsync(string userId)
        {
            return await _context.DownloadHistory
                .Include(dh => dh.User)
                .Include(dh => dh.Game)
                .ThenInclude(g => g.Category)
                .Where(dh => dh.UserId == userId)
                .OrderByDescending(dh => dh.DownloadDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<DownloadHistory>> GetDownloadsByGameAsync(int gameId)
        {
            return await _context.DownloadHistory
                .Include(dh => dh.User)
                .Include(dh => dh.Game)
                .Where(dh => dh.GameId == gameId)
                .OrderByDescending(dh => dh.DownloadDate)
                .ToListAsync();
        }

        public async Task<DownloadHistory?> GetDownloadByUserAndGameAsync(string userId, int gameId)
        {
            return await _context.DownloadHistory
                .Include(dh => dh.User)
                .Include(dh => dh.Game)
                .FirstOrDefaultAsync(dh => dh.UserId == userId && dh.GameId == gameId);
        }

        public async Task<int> GetDownloadsCountByUserAsync(string userId)
        {
            return await _context.DownloadHistory.CountAsync(dh => dh.UserId == userId);
        }

        public async Task<int> GetDownloadsCountByGameAsync(int gameId)
        {
            return await _context.DownloadHistory.CountAsync(dh => dh.GameId == gameId);
        }

        public async Task<IEnumerable<DownloadHistory>> GetRecentDownloadsAsync(int count)
        {
            return await _context.DownloadHistory
                .Include(dh => dh.User)
                .Include(dh => dh.Game)
                .OrderByDescending(dh => dh.DownloadDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetTotalDownloadsCountAsync()
        {
            return await _context.DownloadHistory.CountAsync();
        }

        public async Task<IEnumerable<DownloadHistory>> GetDownloadsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.DownloadHistory
                .Include(dh => dh.User)
                .Include(dh => dh.Game)
                .Where(dh => dh.DownloadDate >= startDate && dh.DownloadDate <= endDate)
                .OrderByDescending(dh => dh.DownloadDate)
                .ToListAsync();
        }
    }
}
