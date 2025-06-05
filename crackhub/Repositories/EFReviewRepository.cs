using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public EFReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Game)
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Game)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Review> CreateAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Reviews.AnyAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Review>> GetReviewsByGameAsync(int gameId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Game)
                .Where(r => r.GameId == gameId)
                .OrderByDescending(r => r.DatePosted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserAsync(string userId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Game)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.DatePosted)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewByUserAndGameAsync(string userId, int gameId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Game)
                .FirstOrDefaultAsync(r => r.UserId == userId && r.GameId == gameId);
        }

        public async Task<double> GetAverageRatingByGameAsync(int gameId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.GameId == gameId)
                .ToListAsync();

            return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
        }

        public async Task<int> GetReviewsCountByGameAsync(int gameId)
        {
            return await _context.Reviews.CountAsync(r => r.GameId == gameId);
        }

        public async Task<IEnumerable<Review>> GetRecentReviewsAsync(int count)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Game)
                .OrderByDescending(r => r.DatePosted)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetTotalReviewsCountAsync()
        {
            return await _context.Reviews.CountAsync();
        }
    }
}
