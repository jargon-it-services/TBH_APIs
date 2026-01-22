using AutoMapper;
using TheBeautyHubCore.DTOs;
using TheBeautyHubCore.Services.Interfaces;
using TheBeautyHubData.Entities;
using TheBeautyHubData.Repositories;
using TheBeautyHubData.Repositories.Interfaces;

namespace TheBeautyHubCore.Services
{
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _repository;
        private readonly IMapper _mapper;

        public PartnerService(IPartnerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PartnerDto> CreateAsync(CreatePartnerDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Partner name cannot be empty.");

            // Check email uniqueness if provided
            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                var existingPartner = await _repository.GetByEmailAsync(dto.Email);
                if (existingPartner != null)
                    throw new InvalidOperationException($"A partner with email '{dto.Email}' already exists.");
            }

            // Validate gender if provided
            if (!string.IsNullOrWhiteSpace(dto.Gender))
            {
                var validGenders = new[] { "Male", "Female", "Other" };
                if (!validGenders.Contains(dto.Gender, StringComparer.OrdinalIgnoreCase))
                    throw new ArgumentException("Gender must be Male, Female, or Other.");
            }

            var entity = _mapper.Map<Partner>(dto);
            var result = await _repository.InsertAsync(entity);
            return _mapper.Map<PartnerDto>(result);
        }

        public async Task<PartnerDto> UpdateAsync(Guid partnerId, UpdatePartnerDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Partner name cannot be empty.");

            var existing = await _repository.GetByIdAsync(partnerId);
            if (existing == null)
                throw new KeyNotFoundException($"Partner with ID {partnerId} not found.");

            // Check email uniqueness if changed
            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != existing.Email)
            {
                var existingPartner = await _repository.GetByEmailAsync(dto.Email);
                if (existingPartner != null)
                    throw new InvalidOperationException($"A partner with email '{dto.Email}' already exists.");
            }

            // Validate gender if provided
            if (!string.IsNullOrWhiteSpace(dto.Gender))
            {
                var validGenders = new[] { "Male", "Female", "Other" };
                if (!validGenders.Contains(dto.Gender, StringComparer.OrdinalIgnoreCase))
                    throw new ArgumentException("Gender must be Male, Female, or Other.");
            }

            existing.Name = dto.Name;
            existing.Type = dto.Type;
            existing.Address = dto.Address;
            existing.Mobile = dto.Mobile;
            existing.Email = dto.Email;
            existing.DateofBirth = dto.DateofBirth;
            existing.Gender = dto.Gender;

            var result = await _repository.UpdateAsync(existing);
            return _mapper.Map<PartnerDto>(result);
        }

        public async Task<bool> DeleteAsync(Guid partnerId)
        {
            var result = await _repository.DeleteAsync(partnerId);
            return result > 0;
        }

        public async Task<PartnerDto?> GetByIdAsync(Guid partnerId)
        {
            var entity = await _repository.GetByIdAsync(partnerId);
            return entity == null ? null : _mapper.Map<PartnerDto>(entity);
        }

        public async Task<IEnumerable<PartnerDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PartnerDto>>(entities);
        }

        public async Task<IEnumerable<PartnerDto>> GetByAccountIdAsync(Guid accountId)
        {
            var entities = await _repository.GetByAccountIdAsync(accountId);
            return _mapper.Map<IEnumerable<PartnerDto>>(entities);
        }

        public async Task<PartnerDto?> GetByEmailAsync(string email)
        {
            var entity = await _repository.GetByEmailAsync(email);
            return entity == null ? null : _mapper.Map<PartnerDto>(entity);
        }
    }
}
