using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFTagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public EFTagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags
                .Include(t => t.GameTags)
                .ThenInclude(gt => gt.Game)
                .ToListAsync();
        }

        public async Task<Tag?> GetByIdAsync(int id)
        {
            return await _context.Tags
                .Include(t => t.GameTags)
                .ThenInclude(gt => gt.Game)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tag?> GetByNameAsync(string name)
        {
            return await _context.Tags
                .Include(t => t.GameTags)
                .ThenInclude(gt => gt.Game)
                .FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<Tag> CreateAsync(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag> UpdateAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return false;

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Tags.AnyAsync(t => t.Id == id);
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await _context.Tags.AnyAsync(t => t.Name == name);
        }

        public async Task<IEnumerable<Tag>> GetTagsWithGamesAsync()
        {
            return await _context.Tags
                .Include(t => t.GameTags)
                .ThenInclude(gt => gt.Game)
                .Where(t => t.GameTags.Any())
                .ToListAsync();
        }

        public async Task<IEnumerable<Tag>> GetTagsByGameAsync(int gameId)
        {
            return await _context.Tags
                .Include(t => t.GameTags)
                .Where(t => t.GameTags.Any(gt => gt.GameId == gameId))
                .ToListAsync();
        }

        public async Task<int> GetTotalTagsCountAsync()
        {
            return await _context.Tags.CountAsync();
        }
    }
}
