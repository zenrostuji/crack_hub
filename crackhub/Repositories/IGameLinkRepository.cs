using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IGameLinkRepository
    {
        Task<IEnumerable<GameLink>> GetAllAsync();
        Task<GameLink?> GetByIdAsync(int id);
        Task<GameLink> CreateAsync(GameLink gameLink);
        Task<GameLink> UpdateAsync(GameLink gameLink);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<GameLink>> GetLinksByGameAsync(int gameId);
        Task<IEnumerable<GameLink>> GetLinksByTypeAsync(string linkType);
        Task<int> GetLinksCountByGameAsync(int gameId);
        Task<bool> DeleteLinksByGameAsync(int gameId);
        Task<int> GetTotalLinksCountAsync();
    }
}
