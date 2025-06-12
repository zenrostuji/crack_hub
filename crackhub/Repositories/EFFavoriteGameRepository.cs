using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFFavoriteGameRepository : IFavoriteGameRepository
    {
        private readonly ApplicationDbContext _context;

        public EFFavoriteGameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FavoriteGame>> GetAllAsync()
        {
            return await _context.FavoriteGames
                .Include(fg => fg.User)
                .Include(fg => fg.Game)
                .ToListAsync();
        }        public async Task<FavoriteGame?> GetByIdAsync(string userId, int gameId)
        {
            return await _context.FavoriteGames
                .Include(fg => fg.User)
                .Include(fg => fg.Game)
                .FirstOrDefaultAsync(fg => fg.UserId == userId && fg.GameId == gameId);
        }

        public async Task<FavoriteGame> CreateAsync(FavoriteGame favoriteGame)
        {
            _context.FavoriteGames.Add(favoriteGame);
            await _context.SaveChangesAsync();
            return favoriteGame;
        }        public async Task<bool> DeleteAsync(string userId, int gameId)
        {
            var favoriteGame = await _context.FavoriteGames
                .FirstOrDefaultAsync(fg => fg.UserId == userId && fg.GameId == gameId);
            if (favoriteGame == null) return false;

            _context.FavoriteGames.Remove(favoriteGame);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(string userId, int gameId)
        {
            return await _context.FavoriteGames
                .AnyAsync(fg => fg.UserId == userId && fg.GameId == gameId);
        }        public async Task<IEnumerable<FavoriteGame>> GetFavoritesByUserAsync(string userId)
        {
            return await _context.FavoriteGames
                .Include(fg => fg.User!)
                .Include(fg => fg.Game!)
                .ThenInclude(g => g.Category)
                .Where(fg => fg.UserId == userId)
                .OrderByDescending(fg => fg.DateAdded)
                .ToListAsync();
        }

        public async Task<FavoriteGame?> GetFavoriteByUserAndGameAsync(string userId, int gameId)
        {
            return await _context.FavoriteGames
                .Include(fg => fg.User)
                .Include(fg => fg.Game)
                .FirstOrDefaultAsync(fg => fg.UserId == userId && fg.GameId == gameId);
        }

        public async Task<bool> IsFavoriteAsync(string userId, int gameId)
        {
            return await _context.FavoriteGames
                .AnyAsync(fg => fg.UserId == userId && fg.GameId == gameId);
        }

        public async Task<bool> RemoveFavoriteAsync(string userId, int gameId)
        {
            var favorite = await _context.FavoriteGames
                .FirstOrDefaultAsync(fg => fg.UserId == userId && fg.GameId == gameId);
            
            if (favorite == null) return false;

            _context.FavoriteGames.Remove(favorite);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetFavoritesCountByUserAsync(string userId)
        {
            return await _context.FavoriteGames.CountAsync(fg => fg.UserId == userId);
        }

        public async Task<int> GetFavoritesCountByGameAsync(int gameId)
        {
            return await _context.FavoriteGames.CountAsync(fg => fg.GameId == gameId);
        }

        public async Task<int> GetTotalFavoritesCountAsync()
        {
            return await _context.FavoriteGames.CountAsync();
        }
    }
}
