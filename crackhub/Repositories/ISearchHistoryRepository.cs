using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface ISearchHistoryRepository
    {
        Task<IEnumerable<SearchHistory>> GetAllAsync();
        Task<SearchHistory?> GetByIdAsync(int id);
        Task<SearchHistory> CreateAsync(SearchHistory searchHistory);
        Task<SearchHistory> UpdateAsync(SearchHistory searchHistory);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<SearchHistory>> GetSearchesByUserAsync(string userId);
        Task<IEnumerable<SearchHistory>> GetRecentSearchesAsync(int count);
        Task<IEnumerable<SearchHistory>> GetPopularSearchTermsAsync(int count);
        Task<int> GetSearchesCountByUserAsync(string userId);
        Task<int> GetTotalSearchesCountAsync();
        Task<bool> DeleteUserSearchHistoryAsync(string userId);
        Task<IEnumerable<SearchHistory>> GetSearchesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
