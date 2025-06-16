using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFPremiumRegisterRepository : IPremiumRegisterRepository
    {
        private readonly ApplicationDbContext _context;

        public EFPremiumRegisterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PremiumRegister>> GetAllAsync()
        {
            return await _context.PremiumRegisters
                .Include(pr => pr.User)
                .Include(pr => pr.Premium)
                .OrderByDescending(pr => pr.RegisterDate)
                .ToListAsync();
        }

        public async Task<PremiumRegister?> GetByIdAsync(int id)
        {
            return await _context.PremiumRegisters
                .Include(pr => pr.User)
                .Include(pr => pr.Premium)
                .FirstOrDefaultAsync(pr => pr.Id == id);
        }

        public async Task<IEnumerable<PremiumRegister>> GetByUserIdAsync(string userId)
        {
            return await _context.PremiumRegisters
                .Include(pr => pr.User)
                .Include(pr => pr.Premium)
                .Where(pr => pr.UserId == userId)
                .OrderByDescending(pr => pr.RegisterDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PremiumRegister>> GetByStatusAsync(int status)
        {
            return await _context.PremiumRegisters
                .Include(pr => pr.User)
                .Include(pr => pr.Premium)
                .Where(pr => pr.Status == status)
                .OrderByDescending(pr => pr.RegisterDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PremiumRegister>> GetByPackageIdAsync(int packageId)
        {
            return await _context.PremiumRegisters
                .Include(pr => pr.User)
                .Include(pr => pr.Premium)
                .Where(pr => pr.PackageId == packageId)
                .OrderByDescending(pr => pr.RegisterDate)
                .ToListAsync();
        }

        public async Task<PremiumRegister> AddAsync(PremiumRegister premiumRegister)
        {
            _context.PremiumRegisters.Add(premiumRegister);
            await _context.SaveChangesAsync();
            return premiumRegister;
        }

        public async Task<PremiumRegister> UpdateAsync(PremiumRegister premiumRegister)
        {
            _context.Entry(premiumRegister).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return premiumRegister;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var premiumRegister = await _context.PremiumRegisters.FindAsync(id);
            if (premiumRegister == null)
                return false;

            _context.PremiumRegisters.Remove(premiumRegister);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PremiumRegisters.AnyAsync(pr => pr.Id == id);
        }

        public async Task<IEnumerable<PremiumRegister>> GetPendingRequestsAsync()
        {
            return await GetByStatusAsync(2); // Status 2: Đang chờ
        }

        public async Task<IEnumerable<PremiumRegister>> GetApprovedRequestsAsync()
        {
            return await GetByStatusAsync(1); // Status 1: Đã xét
        }

        public async Task<IEnumerable<PremiumRegister>> GetRejectedRequestsAsync()
        {
            return await GetByStatusAsync(3); // Status 3: Không xét
        }
    }
}
