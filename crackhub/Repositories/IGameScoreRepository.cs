using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IGameScoreRepository
    {
        Task<GameScore?> GetUserGameScoreAsync(string userId, string gameName);
        Task<IEnumerable<GameScore>> GetTopScoresAsync(string gameName, int count = 10);
        Task<IEnumerable<GameScore>> GetUserScoresAsync(string userId);
        Task<GameScore> AddOrUpdateScoreAsync(GameScore gameScore);
        Task<int> GetUserRankAsync(string userId, string gameName);
        Task<IEnumerable<GameScore>> GetAllTopScoresAsync(int count = 50);
    }
}
