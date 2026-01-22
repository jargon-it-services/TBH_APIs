using TheBeautyHubCore.DTOs;

namespace TheBeautyHubCore.Services.Interfaces
{
    public interface IPartnerService
    {
        Task<PartnerDto> CreateAsync(CreatePartnerDto dto);
        Task<PartnerDto> UpdateAsync(Guid partnerId, UpdatePartnerDto dto);
        Task<bool> DeleteAsync(Guid partnerId);
        Task<PartnerDto?> GetByIdAsync(Guid partnerId);
        Task<IEnumerable<PartnerDto>> GetAllAsync();
        Task<IEnumerable<PartnerDto>> GetByAccountIdAsync(Guid accountId);
        Task<PartnerDto?> GetByEmailAsync(string email);
    }
}
