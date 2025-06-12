using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface ISystemRequirementRepository
    {
        Task<IEnumerable<SystemRequirement>> GetAllAsync();
        Task<SystemRequirement?> GetByIdAsync(int id);
        Task<SystemRequirement> CreateAsync(SystemRequirement systemRequirement);
        Task<SystemRequirement> UpdateAsync(SystemRequirement systemRequirement);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<SystemRequirement>> GetRequirementsByGameAsync(int gameId);
        Task<bool> DeleteRequirementsByGameAsync(int gameId);
        Task<int> GetTotalRequirementsCountAsync();
    }
}
