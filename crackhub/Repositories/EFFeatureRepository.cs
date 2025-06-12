using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFFeatureRepository : IFeatureRepository
    {
        private readonly ApplicationDbContext _context;

        public EFFeatureRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feature>> GetAllAsync()
        {
            return await _context.Features
                .Include(f => f.Game)
                .ToListAsync();
        }

        public async Task<Feature?> GetByIdAsync(int id)
        {
            return await _context.Features
                .Include(f => f.Game)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Feature> CreateAsync(Feature feature)
        {
            _context.Features.Add(feature);
            await _context.SaveChangesAsync();
            return feature;
        }

        public async Task<Feature> UpdateAsync(Feature feature)
        {
            _context.Features.Update(feature);
            await _context.SaveChangesAsync();
            return feature;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var feature = await _context.Features.FindAsync(id);
            if (feature == null) return false;

            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Features.AnyAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Feature>> GetFeaturesByGameAsync(int gameId)
        {
            return await _context.Features
                .Include(f => f.Game)
                .Where(f => f.GameId == gameId)
                .ToListAsync();
        }

        public async Task<bool> DeleteFeaturesByGameAsync(int gameId)
        {
            var features = await _context.Features
                .Where(f => f.GameId == gameId)
                .ToListAsync();

            if (!features.Any()) return false;

            _context.Features.RemoveRange(features);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalFeaturesCountAsync()
        {
            return await _context.Features.CountAsync();
        }
    }
}
