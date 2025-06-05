using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFGameTagRepository : IGameTagRepository
    {
        private readonly ApplicationDbContext _context;

        public EFGameTagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GameTag>> GetAllAsync()
        {
            return await _context.GameTags
                .Include(gt => gt.Game)
                .Include(gt => gt.Tag)
                .ToListAsync();
        }

        public async Task<GameTag?> GetByIdAsync(int gameId, int tagId)
        {
            return await _context.GameTags
                .Include(gt => gt.Game)
                .Include(gt => gt.Tag)
                .FirstOrDefaultAsync(gt => gt.GameId == gameId && gt.TagId == tagId);
        }

        public async Task<GameTag> CreateAsync(GameTag gameTag)
        {
            _context.GameTags.Add(gameTag);
            await _context.SaveChangesAsync();
            return gameTag;
        }

        public async Task<bool> DeleteAsync(int gameId, int tagId)
        {
            var gameTag = await _context.GameTags
                .FirstOrDefaultAsync(gt => gt.GameId == gameId && gt.TagId == tagId);
            if (gameTag == null) return false;

            _context.GameTags.Remove(gameTag);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int gameId, int tagId)
        {
            return await _context.GameTags
                .AnyAsync(gt => gt.GameId == gameId && gt.TagId == tagId);
        }

        public async Task<IEnumerable<GameTag>> GetByGameIdAsync(int gameId)
        {
            return await _context.GameTags
                .Include(gt => gt.Game)
                .Include(gt => gt.Tag)
                .Where(gt => gt.GameId == gameId)
                .ToListAsync();
        }

        public async Task<IEnumerable<GameTag>> GetByTagIdAsync(int tagId)
        {
            return await _context.GameTags
                .Include(gt => gt.Game)
                .Include(gt => gt.Tag)
                .Where(gt => gt.TagId == tagId)
                .ToListAsync();
        }

        public async Task<bool> RemoveAllTagsFromGameAsync(int gameId)
        {
            var gameTags = await _context.GameTags
                .Where(gt => gt.GameId == gameId)
                .ToListAsync();

            if (!gameTags.Any()) return false;

            _context.GameTags.RemoveRange(gameTags);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveGameFromAllTagsAsync(int tagId)
        {
            var gameTags = await _context.GameTags
                .Where(gt => gt.TagId == tagId)
                .ToListAsync();

            if (!gameTags.Any()) return false;

            _context.GameTags.RemoveRange(gameTags);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalGameTagCountAsync()
        {
            return await _context.GameTags.CountAsync();
        }
    }
}
