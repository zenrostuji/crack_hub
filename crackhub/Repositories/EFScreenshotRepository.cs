using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFScreenshotRepository : IScreenshotRepository
    {
        private readonly ApplicationDbContext _context;

        public EFScreenshotRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Screenshot>> GetAllAsync()
        {
            return await _context.Screenshots
                .Include(s => s.Game)
                .ToListAsync();
        }

        public async Task<Screenshot?> GetByIdAsync(int id)
        {
            return await _context.Screenshots
                .Include(s => s.Game)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Screenshot> CreateAsync(Screenshot screenshot)
        {
            _context.Screenshots.Add(screenshot);
            await _context.SaveChangesAsync();
            return screenshot;
        }

        public async Task<Screenshot> UpdateAsync(Screenshot screenshot)
        {
            _context.Screenshots.Update(screenshot);
            await _context.SaveChangesAsync();
            return screenshot;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var screenshot = await _context.Screenshots.FindAsync(id);
            if (screenshot == null) return false;

            _context.Screenshots.Remove(screenshot);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Screenshots.AnyAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Screenshot>> GetScreenshotsByGameAsync(int gameId)
        {
            return await _context.Screenshots
                .Include(s => s.Game)
                .Where(s => s.GameId == gameId)
                .OrderBy(s => s.Id)
                .ToListAsync();
        }

        public async Task<int> GetScreenshotsCountByGameAsync(int gameId)
        {
            return await _context.Screenshots.CountAsync(s => s.GameId == gameId);
        }

        public async Task<bool> DeleteScreenshotsByGameAsync(int gameId)
        {
            var screenshots = await _context.Screenshots
                .Where(s => s.GameId == gameId)
                .ToListAsync();

            if (!screenshots.Any()) return false;

            _context.Screenshots.RemoveRange(screenshots);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalScreenshotsCountAsync()
        {
            return await _context.Screenshots.CountAsync();
        }
    }
}
