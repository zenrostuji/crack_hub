using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IFeatureRepository
    {
        Task<IEnumerable<Feature>> GetAllAsync();
        Task<Feature?> GetByIdAsync(int id);
        Task<Feature> CreateAsync(Feature feature);
        Task<Feature> UpdateAsync(Feature feature);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Feature>> GetFeaturesByGameAsync(int gameId);
        Task<bool> DeleteFeaturesByGameAsync(int gameId);
        Task<int> GetTotalFeaturesCountAsync();
    }
}
