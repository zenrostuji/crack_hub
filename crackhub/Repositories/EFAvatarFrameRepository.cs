using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFAvatarFrameRepository : IAvatarFrameRepository
    {
        private readonly ApplicationDbContext _context;

        public EFAvatarFrameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AvatarFrame>> GetAllAsync()
        {
            return await _context.AvatarFrames
                .Include(af => af.UserAvatarFrames)
                .ThenInclude(uaf => uaf.User)
                .ToListAsync();
        }

        public async Task<AvatarFrame?> GetByIdAsync(int id)
        {
            return await _context.AvatarFrames
                .Include(af => af.UserAvatarFrames)
                .ThenInclude(uaf => uaf.User)
                .FirstOrDefaultAsync(af => af.Id == id);
        }

        public async Task<AvatarFrame> CreateAsync(AvatarFrame avatarFrame)
        {
            _context.AvatarFrames.Add(avatarFrame);
            await _context.SaveChangesAsync();
            return avatarFrame;
        }

        public async Task<AvatarFrame> UpdateAsync(AvatarFrame avatarFrame)
        {
            _context.AvatarFrames.Update(avatarFrame);
            await _context.SaveChangesAsync();
            return avatarFrame;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var avatarFrame = await _context.AvatarFrames.FindAsync(id);
            if (avatarFrame == null) return false;

            _context.AvatarFrames.Remove(avatarFrame);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.AvatarFrames.AnyAsync(af => af.Id == id);
        }

        public async Task<IEnumerable<AvatarFrame>> GetAvailableFramesAsync()
        {
            return await _context.AvatarFrames
                .Where(af => af.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<AvatarFrame>> GetFramesByUserAsync(string userId)
        {
            return await _context.AvatarFrames
                .Include(af => af.UserAvatarFrames)
                .Where(af => af.UserAvatarFrames.Any(uaf => uaf.UserId == userId))
                .ToListAsync();
        }

        public async Task<int> GetTotalFramesCountAsync()
        {
            return await _context.AvatarFrames.CountAsync();
        }
    }
}
