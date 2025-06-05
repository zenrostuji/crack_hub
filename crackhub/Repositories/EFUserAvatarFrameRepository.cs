using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFUserAvatarFrameRepository : IUserAvatarFrameRepository
    {
        private readonly ApplicationDbContext _context;

        public EFUserAvatarFrameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserAvatarFrame>> GetAllAsync()
        {
            return await _context.UserAvatarFrames
                .Include(uaf => uaf.User)
                .Include(uaf => uaf.AvatarFrame)
                .ToListAsync();
        }        public async Task<UserAvatarFrame?> GetByIdAsync(string userId, int frameId)
        {
            return await _context.UserAvatarFrames
                .Include(uaf => uaf.User)
                .Include(uaf => uaf.AvatarFrame)
                .FirstOrDefaultAsync(uaf => uaf.UserId == userId && uaf.FrameId == frameId);
        }

        public async Task<UserAvatarFrame> CreateAsync(UserAvatarFrame userAvatarFrame)
        {
            _context.UserAvatarFrames.Add(userAvatarFrame);
            await _context.SaveChangesAsync();
            return userAvatarFrame;
        }

        public async Task<UserAvatarFrame> UpdateAsync(UserAvatarFrame userAvatarFrame)
        {
            _context.UserAvatarFrames.Update(userAvatarFrame);
            await _context.SaveChangesAsync();
            return userAvatarFrame;
        }        public async Task<bool> DeleteAsync(string userId, int frameId)
        {
            var userAvatarFrame = await _context.UserAvatarFrames
                .FirstOrDefaultAsync(uaf => uaf.UserId == userId && uaf.FrameId == frameId);
            if (userAvatarFrame == null) return false;

            _context.UserAvatarFrames.Remove(userAvatarFrame);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(string userId, int frameId)
        {
            return await _context.UserAvatarFrames
                .AnyAsync(uaf => uaf.UserId == userId && uaf.FrameId == frameId);
        }

        public async Task<IEnumerable<UserAvatarFrame>> GetUserAvatarFramesByUserAsync(string userId)
        {
            return await _context.UserAvatarFrames
                .Include(uaf => uaf.User)
                .Include(uaf => uaf.AvatarFrame)
                .Where(uaf => uaf.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserAvatarFrame?> GetActiveFrameByUserAsync(string userId)
        {
            return await _context.UserAvatarFrames
                .Include(uaf => uaf.User)
                .Include(uaf => uaf.AvatarFrame)
                .FirstOrDefaultAsync(uaf => uaf.UserId == userId && uaf.IsEquipped);
        }

        public async Task<bool> SetActiveFrameAsync(string userId, int avatarFrameId)
        {
            // Deactivate all current frames for the user
            var currentFrames = await _context.UserAvatarFrames
                .Where(uaf => uaf.UserId == userId)
                .ToListAsync();

            foreach (var frame in currentFrames)
            {
                frame.IsEquipped = false;
            }

            // Activate the selected frame
            var targetFrame = await _context.UserAvatarFrames
                .FirstOrDefaultAsync(uaf => uaf.UserId == userId && uaf.FrameId == avatarFrameId);

            if (targetFrame == null) return false;

            targetFrame.IsEquipped = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UserHasFrameAsync(string userId, int avatarFrameId)
        {
            return await _context.UserAvatarFrames
                .AnyAsync(uaf => uaf.UserId == userId && uaf.FrameId == avatarFrameId);
        }

        public async Task<int> GetTotalUserAvatarFramesCountAsync()
        {
            return await _context.UserAvatarFrames.CountAsync();
        }
    }
}
