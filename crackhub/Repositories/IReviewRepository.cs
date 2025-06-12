using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        Task<Review> CreateAsync(Review review);
        Task<Review> UpdateAsync(Review review);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Review>> GetReviewsByGameAsync(int gameId);
        Task<IEnumerable<Review>> GetReviewsByUserAsync(string userId);
        Task<Review?> GetReviewByUserAndGameAsync(string userId, int gameId);
        Task<double> GetAverageRatingByGameAsync(int gameId);
        Task<int> GetReviewsCountByGameAsync(int gameId);
        Task<IEnumerable<Review>> GetRecentReviewsAsync(int count);
        Task<int> GetTotalReviewsCountAsync();
    }
}
