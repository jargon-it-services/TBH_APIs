using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    /// <summary>
    /// Interface for FirmDetails repository operations.
    /// Defines contracts for CRUD operations using stored procedures.
    /// </summary>
    public interface IFirmDetailsRepository
    {
        Task<FirmDetails> InsertFirmDetailsAsync(FirmDetails firmDetails);
        Task<FirmDetails> UpdateFirmDetailsAsync(FirmDetails firmDetails);
        Task<int> DeleteFirmDetailsAsync(Guid firmDetailsId);
        Task<FirmDetails?> GetFirmDetailsByIdAsync(Guid firmDetailsId);
        Task<IEnumerable<FirmDetails>> GetAllFirmDetailsAsync();
        Task<IEnumerable<FirmDetails>> GetFirmDetailsByFirmIdAsync(Guid firmId);
        Task<IEnumerable<FirmDetails>> GetFirmDetailsByUserIdAsync(Guid userId);
        Task<IEnumerable<FirmDetails>> GetFirmDetailsByAccountIdAsync(Guid accountId);
    }
}
