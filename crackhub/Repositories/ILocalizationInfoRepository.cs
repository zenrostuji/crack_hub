using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface ILocalizationInfoRepository
    {
        Task<IEnumerable<LocalizationInfo>> GetAllAsync();
        Task<LocalizationInfo?> GetByIdAsync(int id);
        Task<LocalizationInfo> CreateAsync(LocalizationInfo localizationInfo);
        Task<LocalizationInfo> UpdateAsync(LocalizationInfo localizationInfo);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<LocalizationInfo>> GetByGameIdAsync(int gameId);
        Task<IEnumerable<LocalizationInfo>> GetByLocalizationTypeAsync(string localizationType);
        Task<int> GetTotalLocalizationInfoCountAsync();
        Task<int> GetLocalizationInfoCountByGameAsync(int gameId);
    }
}
