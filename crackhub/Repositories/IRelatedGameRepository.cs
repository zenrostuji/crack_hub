using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IRelatedGameRepository
    {
        Task<IEnumerable<RelatedGame>> GetAllAsync();
        Task<RelatedGame?> GetByIdAsync(int gameId, int relatedGameId);
        Task<RelatedGame> CreateAsync(RelatedGame relatedGame);
        Task<bool> DeleteAsync(int gameId, int relatedGameId);
        Task<bool> ExistsAsync(int gameId, int relatedGameId);
        Task<IEnumerable<RelatedGame>> GetRelatedGamesByGameIdAsync(int gameId);
        Task<IEnumerable<RelatedGame>> GetGamesThatRelateToAsync(int relatedGameId);
        Task<IEnumerable<RelatedGame>> GetByRelationTypeAsync(string relationType);
        Task<bool> RemoveAllRelatedGamesAsync(int gameId);
        Task<int> GetTotalRelatedGameCountAsync();
    }
}
