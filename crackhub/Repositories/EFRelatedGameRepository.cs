using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFRelatedGameRepository : IRelatedGameRepository
    {
        private readonly ApplicationDbContext _context;

        public EFRelatedGameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RelatedGame>> GetAllAsync()
        {
            return await _context.RelatedGames
                .Include(rg => rg.Game)
                .Include(rg => rg.RelatedTo)
                .ToListAsync();
        }

        public async Task<RelatedGame?> GetByIdAsync(int gameId, int relatedGameId)
        {
            return await _context.RelatedGames
                .Include(rg => rg.Game)
                .Include(rg => rg.RelatedTo)
                .FirstOrDefaultAsync(rg => rg.GameId == gameId && rg.RelatedGameId == relatedGameId);
        }

        public async Task<RelatedGame> CreateAsync(RelatedGame relatedGame)
        {
            _context.RelatedGames.Add(relatedGame);
            await _context.SaveChangesAsync();
            return relatedGame;
        }

        public async Task<bool> DeleteAsync(int gameId, int relatedGameId)
        {
            var relatedGame = await _context.RelatedGames
                .FirstOrDefaultAsync(rg => rg.GameId == gameId && rg.RelatedGameId == relatedGameId);
            if (relatedGame == null) return false;

            _context.RelatedGames.Remove(relatedGame);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int gameId, int relatedGameId)
        {
            return await _context.RelatedGames
                .AnyAsync(rg => rg.GameId == gameId && rg.RelatedGameId == relatedGameId);
        }

        public async Task<IEnumerable<RelatedGame>> GetRelatedGamesByGameIdAsync(int gameId)
        {
            return await _context.RelatedGames
                .Include(rg => rg.Game)
                .Include(rg => rg.RelatedTo)
                .Where(rg => rg.GameId == gameId)
                .ToListAsync();
        }

        public async Task<IEnumerable<RelatedGame>> GetGamesThatRelateToAsync(int relatedGameId)
        {
            return await _context.RelatedGames
                .Include(rg => rg.Game)
                .Include(rg => rg.RelatedTo)
                .Where(rg => rg.RelatedGameId == relatedGameId)
                .ToListAsync();
        }

        public async Task<IEnumerable<RelatedGame>> GetByRelationTypeAsync(string relationType)
        {
            return await _context.RelatedGames
                .Include(rg => rg.Game)
                .Include(rg => rg.RelatedTo)
                .Where(rg => rg.RelationType == relationType)
                .ToListAsync();
        }

        public async Task<bool> RemoveAllRelatedGamesAsync(int gameId)
        {
            var relatedGames = await _context.RelatedGames
                .Where(rg => rg.GameId == gameId || rg.RelatedGameId == gameId)
                .ToListAsync();

            if (!relatedGames.Any()) return false;

            _context.RelatedGames.RemoveRange(relatedGames);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalRelatedGameCountAsync()
        {
            return await _context.RelatedGames.CountAsync();
        }
    }
}
