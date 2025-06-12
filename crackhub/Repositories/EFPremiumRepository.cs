using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFPremiumRepository : IPremiumRepository
    {
        private readonly ApplicationDbContext _context;

        public EFPremiumRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Premium>> GetAllAsync()
        {
            return await _context.Premiums
                .Include(p => p.PremiumRegisters)
                .OrderBy(p => p.Id)
                .ToListAsync();
        }

        public async Task<Premium?> GetByIdAsync(int id)
        {
            return await _context.Premiums
                .Include(p => p.PremiumRegisters)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Premium> AddAsync(Premium premium)
        {
            _context.Premiums.Add(premium);
            await _context.SaveChangesAsync();
            return premium;
        }

        public async Task<Premium> UpdateAsync(Premium premium)
        {
            _context.Entry(premium).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return premium;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var premium = await _context.Premiums.FindAsync(id);
            if (premium == null)
                return false;

            _context.Premiums.Remove(premium);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Premiums.AnyAsync(p => p.Id == id);
        }
    }
}
