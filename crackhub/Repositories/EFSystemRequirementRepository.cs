using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFSystemRequirementRepository : ISystemRequirementRepository
    {
        private readonly ApplicationDbContext _context;

        public EFSystemRequirementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SystemRequirement>> GetAllAsync()
        {
            return await _context.SystemRequirements
                .Include(sr => sr.Game)
                .ToListAsync();
        }

        public async Task<SystemRequirement?> GetByIdAsync(int id)
        {
            return await _context.SystemRequirements
                .Include(sr => sr.Game)
                .FirstOrDefaultAsync(sr => sr.Id == id);
        }

        public async Task<SystemRequirement> CreateAsync(SystemRequirement systemRequirement)
        {
            _context.SystemRequirements.Add(systemRequirement);
            await _context.SaveChangesAsync();
            return systemRequirement;
        }

        public async Task<SystemRequirement> UpdateAsync(SystemRequirement systemRequirement)
        {
            _context.SystemRequirements.Update(systemRequirement);
            await _context.SaveChangesAsync();
            return systemRequirement;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var systemRequirement = await _context.SystemRequirements.FindAsync(id);
            if (systemRequirement == null) return false;

            _context.SystemRequirements.Remove(systemRequirement);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.SystemRequirements.AnyAsync(sr => sr.Id == id);
        }

        public async Task<IEnumerable<SystemRequirement>> GetRequirementsByGameAsync(int gameId)
        {
            return await _context.SystemRequirements
                .Include(sr => sr.Game)
                .Where(sr => sr.GameId == gameId)
                .ToListAsync();
        }

        public async Task<bool> DeleteRequirementsByGameAsync(int gameId)
        {
            var requirements = await _context.SystemRequirements
                .Where(sr => sr.GameId == gameId)
                .ToListAsync();

            if (!requirements.Any()) return false;

            _context.SystemRequirements.RemoveRange(requirements);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalRequirementsCountAsync()
        {
            return await _context.SystemRequirements.CountAsync();
        }
    }
}
