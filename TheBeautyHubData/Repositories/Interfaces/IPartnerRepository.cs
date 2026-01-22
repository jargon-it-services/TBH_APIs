using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    public interface IPartnerRepository
    {
        Task<Partner> InsertAsync(Partner partner);
        Task<Partner> UpdateAsync(Partner partner);
        Task<int> DeleteAsync(Guid partnerId);
        Task<Partner?> GetByIdAsync(Guid partnerId);
        Task<IEnumerable<Partner>> GetAllAsync();
        Task<IEnumerable<Partner>> GetByAccountIdAsync(Guid accountId);
        Task<Partner?> GetByEmailAsync(string email);
    }
}
