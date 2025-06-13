using crackhub.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace crackhub.Repositories
{
    public class EFUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public EFUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Reviews)
                .Include(u => u.FavoriteGames)
                .Include(u => u.DownloadHistory)
                .Include(u => u.SearchHistory)
                .Include(u => u.UserAvatarFrames)
                .ThenInclude(uaf => uaf.AvatarFrame)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Reviews)
                .Include(u => u.FavoriteGames)
                .Include(u => u.DownloadHistory)
                .Include(u => u.SearchHistory)
                .Include(u => u.UserAvatarFrames)
                .ThenInclude(uaf => uaf.AvatarFrame)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email || u.NormalizedEmail == email.ToUpper());
        }        public async Task<User?> GetByDisplayNameAsync(string displayName)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.DisplayName == displayName);
        }

        public async Task<User?> AuthenticateAsync(string displayName, string passwordHash)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.DisplayName == displayName && u.PasswordHash == passwordHash);
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email || u.NormalizedEmail == email.ToUpper());
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.RoleId == roleId)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetPremiumUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.PremiumExpiryDate.HasValue && u.PremiumExpiryDate > DateTime.Now)
                .ToListAsync();
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<IEnumerable<User>> GetRecentUsersAsync(int count)
        {
            return await _context.Users
                .Include(u => u.Role)
                .OrderByDescending(u => u.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}
