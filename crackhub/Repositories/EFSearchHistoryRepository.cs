using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFSearchHistoryRepository : ISearchHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public EFSearchHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SearchHistory>> GetAllAsync()
        {
            return await _context.SearchHistory
                .Include(sh => sh.User)
                .ToListAsync();
        }

        public async Task<SearchHistory?> GetByIdAsync(int id)
        {
            return await _context.SearchHistory
                .Include(sh => sh.User)
                .FirstOrDefaultAsync(sh => sh.Id == id);
        }

        public async Task<SearchHistory> CreateAsync(SearchHistory searchHistory)
        {
            _context.SearchHistory.Add(searchHistory);
            await _context.SaveChangesAsync();
            return searchHistory;
        }

        public async Task<SearchHistory> UpdateAsync(SearchHistory searchHistory)
        {
            _context.SearchHistory.Update(searchHistory);
            await _context.SaveChangesAsync();
            return searchHistory;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var searchHistory = await _context.SearchHistory.FindAsync(id);
            if (searchHistory == null) return false;

            _context.SearchHistory.Remove(searchHistory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.SearchHistory.AnyAsync(sh => sh.Id == id);
        }

        public async Task<IEnumerable<SearchHistory>> GetSearchesByUserAsync(string userId)
        {
            return await _context.SearchHistory
                .Include(sh => sh.User)
                .Where(sh => sh.UserId == userId)
                .OrderByDescending(sh => sh.SearchDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SearchHistory>> GetRecentSearchesAsync(int count)
        {
            return await _context.SearchHistory
                .Include(sh => sh.User)
                .OrderByDescending(sh => sh.SearchDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<SearchHistory>> GetPopularSearchTermsAsync(int count)
        {
            return await _context.SearchHistory
                .GroupBy(sh => sh.SearchQuery)
                .Select(g => g.OrderByDescending(sh => sh.SearchDate).First())
                .OrderByDescending(sh => _context.SearchHistory.Count(s => s.SearchQuery == sh.SearchQuery))
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetSearchesCountByUserAsync(string userId)
        {
            return await _context.SearchHistory.CountAsync(sh => sh.UserId == userId);
        }

        public async Task<int> GetTotalSearchesCountAsync()
        {
            return await _context.SearchHistory.CountAsync();
        }

        public async Task<bool> DeleteUserSearchHistoryAsync(string userId)
        {
            var userSearches = await _context.SearchHistory
                .Where(sh => sh.UserId == userId)
                .ToListAsync();

            if (!userSearches.Any()) return false;

            _context.SearchHistory.RemoveRange(userSearches);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SearchHistory>> GetSearchesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.SearchHistory
                .Include(sh => sh.User)
                .Where(sh => sh.SearchDate >= startDate && sh.SearchDate <= endDate)
                .OrderByDescending(sh => sh.SearchDate)
                .ToListAsync();
        }
    }
}
