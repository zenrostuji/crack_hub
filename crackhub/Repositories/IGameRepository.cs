using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game?> GetByIdAsync(int id);
        Task<Game> CreateAsync(Game game);
        Task<Game> UpdateAsync(Game game);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Game>> GetGamesByCategoryAsync(int categoryId);        Task<IEnumerable<Game>> GetGamesByTagAsync(int tagId);
        Task<IEnumerable<Game>> GetGamesByMultipleTagsAsync(List<int> tagIds);
        Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm);
        Task<IEnumerable<Game>> GetTopRatedGamesAsync(int count);
        Task<IEnumerable<Game>> GetRecentGamesAsync(int count);
        Task<IEnumerable<Game>> GetFeaturedGamesAsync();
        Task<IEnumerable<Game>> GetGamesByDeveloperAsync(string developer);
        Task<IEnumerable<Game>> GetGamesByPublisherAsync(string publisher);        Task<int> GetTotalGamesCountAsync();
        Task<int> GetGamesCountByCategoryAsync(int categoryId);
        Task<int> GetGamesCountByTagAsync(int tagId);
        Task<int> GetGamesCountByMultipleTagsAsync(List<int> tagIds);
        Task<int> GetSearchResultsCountAsync(string searchTerm);
        Task UpdateRatingAsync(int gameId, double newRating);
        Task<double> GetAverageRatingAsync();
        Task<IEnumerable<Game>> GetPopularGamesAsync(int count);
    }
}
