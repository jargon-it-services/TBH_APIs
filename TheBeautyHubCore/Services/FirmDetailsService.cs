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
    public class FirmDetailsService : IFirmDetailsService
    {
        private readonly IFirmDetailsRepository _firmDetailsRepository;
        private readonly IMapper _mapper;

        public FirmDetailsService(IFirmDetailsRepository firmDetailsRepository, IMapper mapper)
        {
            _firmDetailsRepository = firmDetailsRepository;
            _mapper = mapper;
        }

        public async Task<FirmDetailsDto> CreateFirmDetailsAsync(CreateFirmDetailsDto createFirmDetailsDto)
        {
            var firmDetails = _mapper.Map<FirmDetails>(createFirmDetailsDto);
            firmDetails.CreatedAt = DateTime.UtcNow;

            var insertedFirmDetails = await _firmDetailsRepository.InsertFirmDetailsAsync(firmDetails);
            return _mapper.Map<FirmDetailsDto>(insertedFirmDetails);
        }

        public async Task<FirmDetailsDto> UpdateFirmDetailsAsync(UpdateFirmDetailsDto updateFirmDetailsDto)
        {
            var existingFirmDetails = await _firmDetailsRepository.GetFirmDetailsByIdAsync(updateFirmDetailsDto.FirmDetailsId);
            if (existingFirmDetails == null)
                throw new KeyNotFoundException($"FirmDetails with ID {updateFirmDetailsDto.FirmDetailsId} not found.");

            var firmDetails = _mapper.Map<FirmDetails>(updateFirmDetailsDto);
            firmDetails.CreatedAt = existingFirmDetails.CreatedAt;

            var updatedFirmDetails = await _firmDetailsRepository.UpdateFirmDetailsAsync(firmDetails);
            return _mapper.Map<FirmDetailsDto>(updatedFirmDetails);
        }

        public async Task<bool> DeleteFirmDetailsAsync(Guid firmDetailsId)
        {
            var existingFirmDetails = await _firmDetailsRepository.GetFirmDetailsByIdAsync(firmDetailsId);
            if (existingFirmDetails == null)
                throw new KeyNotFoundException($"FirmDetails with ID {firmDetailsId} not found.");

            var result = await _firmDetailsRepository.DeleteFirmDetailsAsync(firmDetailsId);
            return result > 0;
        }

        public async Task<FirmDetailsDto?> GetFirmDetailsByIdAsync(Guid firmDetailsId)
        {
            var firmDetails = await _firmDetailsRepository.GetFirmDetailsByIdAsync(firmDetailsId);
            return firmDetails == null ? null : _mapper.Map<FirmDetailsDto>(firmDetails);
        }

        public async Task<IEnumerable<FirmDetailsDto>> GetAllFirmDetailsAsync()
        {
            var firmDetails = await _firmDetailsRepository.GetAllFirmDetailsAsync();
            return _mapper.Map<IEnumerable<FirmDetailsDto>>(firmDetails);
        }

        public async Task<IEnumerable<FirmDetailsDto>> GetFirmDetailsByFirmIdAsync(Guid firmId)
        {
            var firmDetails = await _firmDetailsRepository.GetFirmDetailsByFirmIdAsync(firmId);
            return _mapper.Map<IEnumerable<FirmDetailsDto>>(firmDetails);
        }

        public async Task<IEnumerable<FirmDetailsDto>> GetFirmDetailsByUserIdAsync(Guid userId)
        {
            var firmDetails = await _firmDetailsRepository.GetFirmDetailsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<FirmDetailsDto>>(firmDetails);
        }

        public async Task<IEnumerable<FirmDetailsDto>> GetFirmDetailsByAccountIdAsync(Guid accountId)
        {
            var firmDetails = await _firmDetailsRepository.GetFirmDetailsByAccountIdAsync(accountId);
            return _mapper.Map<IEnumerable<FirmDetailsDto>>(firmDetails);
        }
    }
}
