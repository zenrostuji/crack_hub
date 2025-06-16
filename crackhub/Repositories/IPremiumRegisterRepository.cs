using crackhub.Models.Data;

namespace crackhub.Repositories
{
    public interface IPremiumRegisterRepository
    {
        Task<IEnumerable<PremiumRegister>> GetAllAsync();
        Task<PremiumRegister?> GetByIdAsync(int id);
        Task<IEnumerable<PremiumRegister>> GetByUserIdAsync(string userId);
        Task<IEnumerable<PremiumRegister>> GetByStatusAsync(int status);
        Task<IEnumerable<PremiumRegister>> GetByPackageIdAsync(int packageId);
        Task<PremiumRegister> AddAsync(PremiumRegister premiumRegister);
        Task<PremiumRegister> UpdateAsync(PremiumRegister premiumRegister);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<PremiumRegister>> GetPendingRequestsAsync();
        Task<IEnumerable<PremiumRegister>> GetApprovedRequestsAsync();
        Task<IEnumerable<PremiumRegister>> GetRejectedRequestsAsync();
    }
}
