using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IGameTagRepository
    {
        Task<IEnumerable<GameTag>> GetAllAsync();
        Task<GameTag?> GetByIdAsync(int gameId, int tagId);
        Task<GameTag> CreateAsync(GameTag gameTag);
        Task<bool> DeleteAsync(int gameId, int tagId);
        Task<bool> ExistsAsync(int gameId, int tagId);
        Task<IEnumerable<GameTag>> GetByGameIdAsync(int gameId);
        Task<IEnumerable<GameTag>> GetByTagIdAsync(int tagId);
        Task<bool> RemoveAllTagsFromGameAsync(int gameId);
        Task<bool> RemoveGameFromAllTagsAsync(int tagId);
        Task<int> GetTotalGameTagCountAsync();
    }
}
