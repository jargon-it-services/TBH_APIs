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
    /// <summary>
    /// Service implementation for Wallet business logic.
    /// Handles validation and business rules for wallet operations.
    /// </summary>
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IMapper _mapper;

        public WalletService(IWalletRepository walletRepository, IMapper mapper)
        {
            _walletRepository = walletRepository;
            _mapper = mapper;
        }

        public async Task<WalletDto> CreateWalletAsync(CreateWalletDto createWalletDto)
        {
            if (createWalletDto == null)
                throw new ArgumentNullException(nameof(createWalletDto));

            if (createWalletDto.Amount < 0)
                throw new ArgumentException("Wallet amount cannot be negative.");

            if (string.IsNullOrWhiteSpace(createWalletDto.WalletType))
                throw new ArgumentException("Wallet type is required.");

            var wallet = _mapper.Map<Wallet>(createWalletDto);
            var createdWallet = await _walletRepository.InsertWalletAsync(wallet);
            return _mapper.Map<WalletDto>(createdWallet);
        }

        public async Task<WalletDto> UpdateWalletAsync(UpdateWalletDto updateWalletDto)
        {
            if (updateWalletDto == null)
                throw new ArgumentNullException(nameof(updateWalletDto));

            if (updateWalletDto.Amount < 0)
                throw new ArgumentException("Wallet amount cannot be negative.");

            if (string.IsNullOrWhiteSpace(updateWalletDto.WalletType))
                throw new ArgumentException("Wallet type is required.");

            var existingWallet = await _walletRepository.GetWalletByIdAsync(updateWalletDto.WalletId);
            if (existingWallet == null)
                throw new KeyNotFoundException($"Wallet with ID {updateWalletDto.WalletId} not found.");

            var wallet = _mapper.Map<Wallet>(updateWalletDto);
            var updatedWallet = await _walletRepository.UpdateWalletAsync(wallet);
            return _mapper.Map<WalletDto>(updatedWallet);
        }

        public async Task<bool> DeleteWalletAsync(Guid walletId)
        {
            var existingWallet = await _walletRepository.GetWalletByIdAsync(walletId);
            if (existingWallet == null)
                throw new KeyNotFoundException($"Wallet with ID {walletId} not found.");

            var result = await _walletRepository.DeleteWalletAsync(walletId);
            return result > 0;
        }

        public async Task<WalletDto?> GetWalletByIdAsync(Guid walletId)
        {
            var wallet = await _walletRepository.GetWalletByIdAsync(walletId);
            return wallet != null ? _mapper.Map<WalletDto>(wallet) : null;
        }

        public async Task<IEnumerable<WalletDto>> GetWalletsByAccountIdAsync(Guid accountId)
        {
            var wallets = await _walletRepository.GetWalletsByAccountIdAsync(accountId);
            return _mapper.Map<IEnumerable<WalletDto>>(wallets);
        }

        public async Task<IEnumerable<WalletDto>> GetAllWalletsAsync()
        {
            var wallets = await _walletRepository.GetAllWalletsAsync();
            return _mapper.Map<IEnumerable<WalletDto>>(wallets);
        }
    }
}
