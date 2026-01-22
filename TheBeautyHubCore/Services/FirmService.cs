using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class FirmService : IFirmService
    {
        private readonly IFirmRepository _firmRepository;
        private readonly IMapper _mapper;

        public FirmService(IFirmRepository firmRepository, IMapper mapper)
        {
            _firmRepository = firmRepository;
            _mapper = mapper;
        }

        public async Task<FirmDto> CreateFirmAsync(CreateFirmDto createFirmDto)
        {
            if (string.IsNullOrWhiteSpace(createFirmDto.FirmName))
                throw new ArgumentException("Firm name is required.");

            var firm = _mapper.Map<Firm>(createFirmDto);
            firm.CreatedAt = DateTime.UtcNow;

            var insertedFirm = await _firmRepository.InsertFirmAsync(firm);
            return _mapper.Map<FirmDto>(insertedFirm);
        }

        public async Task<FirmDto> UpdateFirmAsync(UpdateFirmDto updateFirmDto)
        {
            if (string.IsNullOrWhiteSpace(updateFirmDto.FirmName))
                throw new ArgumentException("Firm name is required.");

            var existingFirm = await _firmRepository.GetFirmByIdAsync(updateFirmDto.FirmId);
            if (existingFirm == null)
                throw new KeyNotFoundException($"Firm with ID {updateFirmDto.FirmId} not found.");

            var firm = _mapper.Map<Firm>(updateFirmDto);
            firm.AccountId = existingFirm.AccountId;
            firm.CreatedAt = existingFirm.CreatedAt;

            var updatedFirm = await _firmRepository.UpdateFirmAsync(firm);
            return _mapper.Map<FirmDto>(updatedFirm);
        }

        public async Task<bool> DeleteFirmAsync(Guid firmId)
        {
            var existingFirm = await _firmRepository.GetFirmByIdAsync(firmId);
            if (existingFirm == null)
                throw new KeyNotFoundException($"Firm with ID {firmId} not found.");

            var result = await _firmRepository.DeleteFirmAsync(firmId);
            return result > 0;
        }

        public async Task<FirmDto?> GetFirmByIdAsync(Guid firmId)
        {
            var firm = await _firmRepository.GetFirmByIdAsync(firmId);
            return firm == null ? null : _mapper.Map<FirmDto>(firm);
        }

        public async Task<IEnumerable<FirmDto>> GetAllFirmsAsync()
        {
            var firms = await _firmRepository.GetAllFirmsAsync();
            return _mapper.Map<IEnumerable<FirmDto>>(firms);
        }

        public async Task<IEnumerable<FirmDto>> GetFirmsByAccountIdAsync(Guid accountId)
        {
            var firms = await _firmRepository.GetFirmsByAccountIdAsync(accountId);
            return _mapper.Map<IEnumerable<FirmDto>>(firms);
        }
    }
}
