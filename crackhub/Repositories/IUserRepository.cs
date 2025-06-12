using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> EmailExistsAsync(string email);
        Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);
        Task<IEnumerable<User>> GetPremiumUsersAsync();
        Task<User?> GetByDisplayNameAsync(string displayName);
        Task<User?> AuthenticateAsync(string displayName, string passwordHash);
        Task<int> GetTotalUsersCountAsync();
        Task<IEnumerable<User>> GetRecentUsersAsync(int count);
    }
}
