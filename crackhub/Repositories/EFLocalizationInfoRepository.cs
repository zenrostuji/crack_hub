using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFLocalizationInfoRepository : ILocalizationInfoRepository
    {
        private readonly ApplicationDbContext _context;

        public EFLocalizationInfoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LocalizationInfo>> GetAllAsync()
        {
            return await _context.LocalizationInfos
                .Include(li => li.Game)
                .ToListAsync();
        }

        public async Task<LocalizationInfo?> GetByIdAsync(int id)
        {
            return await _context.LocalizationInfos
                .Include(li => li.Game)
                .FirstOrDefaultAsync(li => li.Id == id);
        }

        public async Task<LocalizationInfo> CreateAsync(LocalizationInfo localizationInfo)
        {
            _context.LocalizationInfos.Add(localizationInfo);
            await _context.SaveChangesAsync();
            return localizationInfo;
        }

        public async Task<LocalizationInfo> UpdateAsync(LocalizationInfo localizationInfo)
        {
            _context.LocalizationInfos.Update(localizationInfo);
            await _context.SaveChangesAsync();
            return localizationInfo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var localizationInfo = await _context.LocalizationInfos.FindAsync(id);
            if (localizationInfo == null) return false;

            _context.LocalizationInfos.Remove(localizationInfo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.LocalizationInfos.AnyAsync(li => li.Id == id);
        }

        public async Task<IEnumerable<LocalizationInfo>> GetByGameIdAsync(int gameId)
        {
            return await _context.LocalizationInfos
                .Include(li => li.Game)
                .Where(li => li.GameId == gameId)
                .ToListAsync();
        }

        public async Task<IEnumerable<LocalizationInfo>> GetByLocalizationTypeAsync(string localizationType)
        {
            return await _context.LocalizationInfos
                .Include(li => li.Game)
                .Where(li => li.LocalizationType == localizationType)
                .ToListAsync();
        }

        public async Task<int> GetTotalLocalizationInfoCountAsync()
        {
            return await _context.LocalizationInfos.CountAsync();
        }

        public async Task<int> GetLocalizationInfoCountByGameAsync(int gameId)
        {
            return await _context.LocalizationInfos.CountAsync(li => li.GameId == gameId);
        }
    }
}
