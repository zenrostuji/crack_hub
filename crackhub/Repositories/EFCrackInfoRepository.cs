using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFCrackInfoRepository : ICrackInfoRepository
    {
        private readonly ApplicationDbContext _context;

        public EFCrackInfoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CrackInfo>> GetAllAsync()
        {
            return await _context.CrackInfo
                .Include(ci => ci.Game)
                .ToListAsync();
        }

        public async Task<CrackInfo?> GetByIdAsync(int id)
        {
            return await _context.CrackInfo
                .Include(ci => ci.Game)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        public async Task<CrackInfo> CreateAsync(CrackInfo crackInfo)
        {
            _context.CrackInfo.Add(crackInfo);
            await _context.SaveChangesAsync();
            return crackInfo;
        }

        public async Task<CrackInfo> UpdateAsync(CrackInfo crackInfo)
        {
            _context.CrackInfo.Update(crackInfo);
            await _context.SaveChangesAsync();
            return crackInfo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var crackInfo = await _context.CrackInfo.FindAsync(id);
            if (crackInfo == null) return false;

            _context.CrackInfo.Remove(crackInfo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.CrackInfo.AnyAsync(ci => ci.Id == id);
        }

        public async Task<IEnumerable<CrackInfo>> GetCrackInfosByGameAsync(int gameId)
        {
            return await _context.CrackInfo
                .Include(ci => ci.Game)
                .Where(ci => ci.GameId == gameId)
                .OrderByDescending(ci => ci.ReleaseDate)
                .ToListAsync();
        }

        public async Task<CrackInfo?> GetLatestCrackInfoByGameAsync(int gameId)
        {
            return await _context.CrackInfo
                .Include(ci => ci.Game)
                .Where(ci => ci.GameId == gameId)
                .OrderByDescending(ci => ci.ReleaseDate)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteCrackInfosByGameAsync(int gameId)
        {
            var crackInfos = await _context.CrackInfo
                .Where(ci => ci.GameId == gameId)
                .ToListAsync();

            if (!crackInfos.Any()) return false;

            _context.CrackInfo.RemoveRange(crackInfos);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalCrackInfosCountAsync()
        {
            return await _context.CrackInfo.CountAsync();
        }
    }
}
