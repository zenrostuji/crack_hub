using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IUserAvatarFrameRepository
    {
        Task<IEnumerable<UserAvatarFrame>> GetAllAsync();
        Task<UserAvatarFrame?> GetByIdAsync(string userId, int frameId);
        Task<UserAvatarFrame> CreateAsync(UserAvatarFrame userAvatarFrame);
        Task<UserAvatarFrame> UpdateAsync(UserAvatarFrame userAvatarFrame);
        Task<bool> DeleteAsync(string userId, int frameId);
        Task<bool> ExistsAsync(string userId, int frameId);
        Task<IEnumerable<UserAvatarFrame>> GetUserAvatarFramesByUserAsync(string userId);
        Task<UserAvatarFrame?> GetActiveFrameByUserAsync(string userId);
        Task<bool> SetActiveFrameAsync(string userId, int avatarFrameId);
        Task<bool> UserHasFrameAsync(string userId, int avatarFrameId);
        Task<int> GetTotalUserAvatarFramesCountAsync();
    }
}
