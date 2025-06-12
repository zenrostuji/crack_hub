using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IFavoriteGameRepository
    {
        Task<IEnumerable<FavoriteGame>> GetAllAsync();
        Task<FavoriteGame?> GetByIdAsync(string userId, int gameId);
        Task<FavoriteGame> CreateAsync(FavoriteGame favoriteGame);
        Task<bool> DeleteAsync(string userId, int gameId);
        Task<bool> ExistsAsync(string userId, int gameId);
        Task<IEnumerable<FavoriteGame>> GetFavoritesByUserAsync(string userId);
        Task<FavoriteGame?> GetFavoriteByUserAndGameAsync(string userId, int gameId);
        Task<bool> IsFavoriteAsync(string userId, int gameId);
        Task<bool> RemoveFavoriteAsync(string userId, int gameId);
        Task<int> GetFavoritesCountByUserAsync(string userId);
        Task<int> GetFavoritesCountByGameAsync(int gameId);
        Task<int> GetTotalFavoritesCountAsync();
    }
}
