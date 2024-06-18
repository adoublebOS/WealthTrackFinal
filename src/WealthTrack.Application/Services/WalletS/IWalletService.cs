using WealthTrack.Application.DTO;

namespace WealthTrack.Application.Services.WalletS;

public interface IWalletService
{
    Task<IEnumerable<WalletDto>> GetAllAsync(int userId);
    Task CreateWalletAsync(CreateWalletDto dto, int userId);
    Task UpdateWalletAsync(int id, UpdateWalletDto dto, int userId);
    Task DeleteWalletAsync(int id, int userId);

    Task<IEnumerable<WalletLimitDto>> GetWalletLimitsAsync(int walletId, int userId);
    Task<WalletLimitDto> GetWalletLimitByMonthAsync(int walletId, int userId, DateTime month);
    Task CreateWalletLimitAsync(CreateWalletLimitDto dto, int userId);
    Task UpdateWalletLimitAsync(int id, UpdateWalletLimitDto dto, int userId);
    Task DeleteWalletLimitAsync(int id, int userId);
}