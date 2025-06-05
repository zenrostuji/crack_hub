using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IAvatarFrameRepository
    {
        Task<IEnumerable<AvatarFrame>> GetAllAsync();
        Task<AvatarFrame?> GetByIdAsync(int id);
        Task<AvatarFrame> CreateAsync(AvatarFrame avatarFrame);
        Task<AvatarFrame> UpdateAsync(AvatarFrame avatarFrame);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<AvatarFrame>> GetAvailableFramesAsync();
        Task<IEnumerable<AvatarFrame>> GetFramesByUserAsync(string userId);
        Task<int> GetTotalFramesCountAsync();
    }
}
