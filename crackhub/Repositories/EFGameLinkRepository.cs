using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFGameLinkRepository : IGameLinkRepository
    {
        private readonly ApplicationDbContext _context;

        public EFGameLinkRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GameLink>> GetAllAsync()
        {
            return await _context.GameLinks
                .Include(gl => gl.Game)
                .ToListAsync();
        }

        public async Task<GameLink?> GetByIdAsync(int id)
        {
            return await _context.GameLinks
                .Include(gl => gl.Game)
                .FirstOrDefaultAsync(gl => gl.Id == id);
        }

        public async Task<GameLink> CreateAsync(GameLink gameLink)
        {
            _context.GameLinks.Add(gameLink);
            await _context.SaveChangesAsync();
            return gameLink;
        }

        public async Task<GameLink> UpdateAsync(GameLink gameLink)
        {
            _context.GameLinks.Update(gameLink);
            await _context.SaveChangesAsync();
            return gameLink;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var gameLink = await _context.GameLinks.FindAsync(id);
            if (gameLink == null) return false;

            _context.GameLinks.Remove(gameLink);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.GameLinks.AnyAsync(gl => gl.Id == id);
        }

        public async Task<IEnumerable<GameLink>> GetLinksByGameAsync(int gameId)
        {
            return await _context.GameLinks
                .Include(gl => gl.Game)
                .Where(gl => gl.GameId == gameId)
                .OrderBy(gl => gl.LinkName)
                .ToListAsync();
        }

        public async Task<IEnumerable<GameLink>> GetLinksByTypeAsync(string linkType)
        {
            return await _context.GameLinks
                .Include(gl => gl.Game)
                .Where(gl => gl.LinkName == linkType)
                .ToListAsync();
        }

        public async Task<int> GetLinksCountByGameAsync(int gameId)
        {
            return await _context.GameLinks.CountAsync(gl => gl.GameId == gameId);
        }

        public async Task<bool> DeleteLinksByGameAsync(int gameId)
        {
            var gameLinks = await _context.GameLinks
                .Where(gl => gl.GameId == gameId)
                .ToListAsync();

            if (!gameLinks.Any()) return false;

            _context.GameLinks.RemoveRange(gameLinks);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalLinksCountAsync()
        {
            return await _context.GameLinks.CountAsync();
        }
    }
}
