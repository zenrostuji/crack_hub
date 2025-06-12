using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFGameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _context;

        public EFGameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games
                .Include(g => g.Category)
                .Include(g => g.Screenshots)
                .Include(g => g.GameLinks)
                .Include(g => g.SystemRequirements)
                .Include(g => g.Features)
                .Include(g => g.GameTags)
                .ThenInclude(gt => gt.Tag)
                .ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games
                .Include(g => g.Category)
                .Include(g => g.Screenshots)
                .Include(g => g.GameLinks)
                .Include(g => g.SystemRequirements)
                .Include(g => g.Features)
                .Include(g => g.GameTags)
                .ThenInclude(gt => gt.Tag)
                .Include(g => g.Reviews)
                .ThenInclude(r => r.User)
                .Include(g => g.FavoriteGames)
                .Include(g => g.DownloadHistory)
                .Include(g => g.RelatedGames)
                .Include(g => g.CrackInfos)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Game> CreateAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<Game> UpdateAsync(Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return false;

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Games.AnyAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Game>> GetGamesByCategoryAsync(int categoryId)
        {
            return await _context.Games
                .Include(g => g.Category)
                .Include(g => g.Screenshots)
                .Where(g => g.CategoryId == categoryId)
                .ToListAsync();
        }        public async Task<IEnumerable<Game>> GetGamesByTagAsync(int tagId)
        {
            return await _context.Games
                .Include(g => g.Category)
                .Include(g => g.Screenshots)
                .Include(g => g.GameTags)
                .ThenInclude(gt => gt.Tag)
                .Where(g => g.GameTags.Any(gt => gt.TagId == tagId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetGamesByMultipleTagsAsync(List<int> tagIds)
        {
            return await _context.Games
                .Include(g => g.Category)
                .Include(g => g.Screenshots)
                .Include(g => g.GameTags)
                .ThenInclude(gt => gt.Tag)
                .Where(g => tagIds.All(tagId => g.GameTags.Any(gt => gt.TagId == tagId)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm)
        {
            return await _context.Games
                .Include(g => g.Category)
                .Include(g => g.Screenshots)
                .Where(g => g.Title.Contains(searchTerm) ||
                           g.ShortDescription!.Contains(searchTerm) ||
                           g.FullDescription!.Contains(searchTerm) ||
                           g.Developer!.Contains(searchTerm) ||
                           g.Publisher!.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetTopRatedGamesAsync(int count)
        {
            return await _context.Games
                .Include(g => g.Category)
                .Include(g => g.Screenshots)
                .OrderByDescending(g => g.Rating)
                .Take(count)
                .ToListAsync();
        }        public async Task<IEnumerable<Game>> GetRecentGamesAsync(int count)
        {
            return await _context.Games
                .Include(g => g.Category)
                .Include(g => g.Screenshots)
                .OrderByDescending(g => g.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetFeaturedGamesAsync()
        {
            return await _context.Games
                .Include(g => g.Category)
                .Include(g => g.Screenshots)
                .Where(g => g.IsFeatured)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetGamesByDeveloperAsync(string developer)
        {
            return await _context.Games
                .Include(g => g.Category)
                .Include(g => g.Screenshots)
                .Where(g => g.Developer == developer)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetGamesByPublisherAsync(string publisher)
        {
            return await _context.Games
                .Include(g => g.Category)
                .Include(g => g.Screenshots)
                .Where(g => g.Publisher == publisher)
                .ToListAsync();
        }

        public async Task<int> GetTotalGamesCountAsync()
        {
            return await _context.Games.CountAsync();
        }

        public async Task<int> GetGamesCountByCategoryAsync(int categoryId)
        {
            return await _context.Games.CountAsync(g => g.CategoryId == categoryId);
        }        public async Task UpdateRatingAsync(int gameId, double newRating)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game != null)
            {
                game.Rating = newRating;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<double> GetAverageRatingAsync()
        {
            var gamesWithRatings = await _context.Games
                .Where(g => g.AverageRating > 0)
                .Select(g => g.AverageRating)
                .ToListAsync();

            if (gamesWithRatings.Any())
            {
                return gamesWithRatings.Average();
            }
            return 0;
        }        public async Task<IEnumerable<Game>> GetPopularGamesAsync(int count)
        {
            return await _context.Games
                .Include(g => g.Category)
                .OrderByDescending(g => g.Downloads)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetGamesCountByTagAsync(int tagId)
        {
            return await _context.Games
                .Where(g => g.GameTags.Any(gt => gt.TagId == tagId))
                .CountAsync();
        }

        public async Task<int> GetGamesCountByMultipleTagsAsync(List<int> tagIds)
        {
            return await _context.Games
                .Where(g => tagIds.All(tagId => g.GameTags.Any(gt => gt.TagId == tagId)))
                .CountAsync();
        }

        public async Task<int> GetSearchResultsCountAsync(string searchTerm)
        {
            return await _context.Games
                .Where(g => g.Title.Contains(searchTerm) ||
                           g.ShortDescription!.Contains(searchTerm) ||
                           g.FullDescription!.Contains(searchTerm) ||
                           g.Developer!.Contains(searchTerm) ||
                           g.Publisher!.Contains(searchTerm))
                .CountAsync();
        }
    }
}
