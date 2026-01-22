using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBeautyHubData.Entities;

namespace TheBeautyHubData.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Firm repository operations.
    /// Defines contracts for CRUD operations using stored procedures.
    /// </summary>
    public interface IFirmRepository
    {
        Task<Firm> InsertFirmAsync(Firm firm);
        Task<Firm> UpdateFirmAsync(Firm firm);
        Task<int> DeleteFirmAsync(Guid firmId);
        Task<Firm?> GetFirmByIdAsync(Guid firmId);
        Task<IEnumerable<Firm>> GetAllFirmsAsync();
        Task<IEnumerable<Firm>> GetFirmsByAccountIdAsync(Guid accountId);
    }
}
