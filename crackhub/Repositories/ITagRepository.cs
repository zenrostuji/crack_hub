using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag?> GetByIdAsync(int id);
        Task<Tag?> GetByNameAsync(string name);
        Task<Tag> CreateAsync(Tag tag);
        Task<Tag> UpdateAsync(Tag tag);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> NameExistsAsync(string name);
        Task<IEnumerable<Tag>> GetTagsWithGamesAsync();
        Task<IEnumerable<Tag>> GetTagsByGameAsync(int gameId);
        Task<int> GetTotalTagsCountAsync();
    }
}
