using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface ICrackInfoRepository
    {
        Task<IEnumerable<CrackInfo>> GetAllAsync();
        Task<CrackInfo?> GetByIdAsync(int id);
        Task<CrackInfo> CreateAsync(CrackInfo crackInfo);
        Task<CrackInfo> UpdateAsync(CrackInfo crackInfo);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<CrackInfo>> GetCrackInfosByGameAsync(int gameId);
        Task<CrackInfo?> GetLatestCrackInfoByGameAsync(int gameId);
        Task<bool> DeleteCrackInfosByGameAsync(int gameId);
        Task<int> GetTotalCrackInfosCountAsync();
    }
}
