using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFGameScoreRepository : IGameScoreRepository
    {
        private readonly ApplicationDbContext _context;

        public EFGameScoreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GameScore?> GetUserGameScoreAsync(string userId, string gameName)
        {
            return await _context.GameScores
                .Include(gs => gs.User)
                .FirstOrDefaultAsync(gs => gs.UserId == userId && gs.GameName == gameName);
        }

        public async Task<IEnumerable<GameScore>> GetTopScoresAsync(string gameName, int count = 10)
        {
            return await _context.GameScores
                .Include(gs => gs.User)
                .Where(gs => gs.GameName == gameName)
                .OrderByDescending(gs => gs.Score)
                .ThenByDescending(gs => gs.Level)
                .ThenByDescending(gs => gs.SurvivalTime)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<GameScore>> GetUserScoresAsync(string userId)
        {
            return await _context.GameScores
                .Include(gs => gs.User)
                .Where(gs => gs.UserId == userId)
                .OrderByDescending(gs => gs.Score)
                .ToListAsync();
        }

        public async Task<GameScore> AddOrUpdateScoreAsync(GameScore gameScore)
        {
            var existingScore = await GetUserGameScoreAsync(gameScore.UserId, gameScore.GameName);

            if (existingScore == null)
            {
                // Thêm điểm số mới
                _context.GameScores.Add(gameScore);
            }
            else
            {
                // Cập nhật nếu điểm số mới cao hơn
                if (gameScore.Score > existingScore.Score)
                {
                    existingScore.Score = gameScore.Score;
                    existingScore.Level = gameScore.Level;
                    existingScore.EnemiesKilled = gameScore.EnemiesKilled;
                    existingScore.SurvivalTime = gameScore.SurvivalTime;
                    existingScore.UpdatedAt = DateTime.Now;
                }
                gameScore = existingScore;
            }

            await _context.SaveChangesAsync();
            return gameScore;
        }

        public async Task<int> GetUserRankAsync(string userId, string gameName)
        {
            var userScore = await GetUserGameScoreAsync(userId, gameName);
            if (userScore == null) return 0;

            var rank = await _context.GameScores
                .Where(gs => gs.GameName == gameName && gs.Score > userScore.Score)
                .CountAsync();

            return rank + 1;
        }

        public async Task<IEnumerable<GameScore>> GetAllTopScoresAsync(int count = 50)
        {
            return await _context.GameScores
                .Include(gs => gs.User)
                .OrderByDescending(gs => gs.Score)
                .ThenByDescending(gs => gs.Level)
                .ThenByDescending(gs => gs.SurvivalTime)
                .Take(count)
                .ToListAsync();
        }
    }
}
