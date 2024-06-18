using WealthTrack.Application.DTO;
using WealthTrack.Domain.Entities;
using WealthTrack.Domain.Exceptions;
using WealthTrack.Infrastructure.Repository;

namespace WealthTrack.Application.Services.WalletS;

public class WalletService : IWalletService
{
    private readonly IRepository<Wallet> _walletRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<WalletLimit> _walletLimitRepository;

    public WalletService(IRepository<Wallet> walletRepository, 
        IRepository<User> userRepository, 
        IRepository<WalletLimit> walletLimitRepository)
    {
        _walletRepository = walletRepository;
        _userRepository = userRepository;
        _walletLimitRepository = walletLimitRepository;
    }

    public async Task<IEnumerable<WalletDto>> GetAllAsync(int userId)
    {
        var wallets = await _walletRepository
            .GetAllByFilterAsync(w => w.User!.Id == userId,
                w => w.User!);

        return wallets
            .OrderByDescending(w => w.CreatedAt)
            .Select(w => w.AsDto())
            .ToList();
    }

    public async Task CreateWalletAsync(CreateWalletDto dto, int userId)
    {
        var user = await _userRepository.GetOneAsync(u => u.Id == userId) ?? 
                   throw new UserNotFoundException();
        var wallet = dto.AsEntity(user);
        
        _userRepository.Attach(user);
        
        await _walletRepository.CreateAsync(wallet);
        await _walletRepository.SaveChangesAsync();
    }

    public async Task UpdateWalletAsync(int id, UpdateWalletDto dto, int userId)
    {
        var user = await _userRepository.GetOneAsync(u => u.Id == userId) ?? 
                   throw new UserNotFoundException();
        var wallet = await _walletRepository.GetOneAsync(
                         w => w.Id == id,
                         w => w.User!) ??
                     throw new WalletNotFoundException();
        
        if (wallet.User!.Id != userId) throw new AccessDeniedException();
        
        dto.UpdateEntity(wallet);
        
        _userRepository.Attach(user);
        
        await _walletRepository.UpdateAsync(wallet.Id, wallet);
        await _walletRepository.SaveChangesAsync();
    }

    public async Task DeleteWalletAsync(int id, int userId)
    {
        var user = await _userRepository.GetOneAsync(u => u.Id == userId) ?? 
                   throw new UserNotFoundException();
        var wallet = await _walletRepository.GetOneAsync(
                         w => w.Id == id,
                         w => w.User!) ??
                     throw new WalletNotFoundException();
        
        if (wallet.User!.Id != userId) throw new AccessDeniedException();

        await _walletRepository.SoftDeleteAsync(id);
        await _walletRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<WalletLimitDto>> GetWalletLimitsAsync(int walletId, int userId)
    {
        var wallet = await _walletRepository.GetOneAsync(w => w.Id == walletId && w.User!.Id == userId, 
                         w => w.User!)
            ?? throw new WalletNotFoundException();
        
        var walletLimits = await _walletLimitRepository.GetAllByFilterAsync(wl => wl.Wallet!.Id == walletId);
        return walletLimits.Select(wl => wl.AsDto()).ToList();
    }

    public async Task<WalletLimitDto> GetWalletLimitByMonthAsync(int walletId, int userId, DateTime month)
    {
        var wallet = await _walletRepository.GetOneAsync(w => w.Id == walletId && w.User!.Id == userId, 
                         w => w.User!)
                     ?? throw new WalletNotFoundException();

        var walletLimit = await _walletLimitRepository.GetOneAsync(wl => wl.Wallet!.Id == walletId 
                                                                         && wl.Month.Month == month.Month)
                          ?? throw new WalletLimitNotFoundException();

        return walletLimit.AsDto();
    }

    public async Task CreateWalletLimitAsync(CreateWalletLimitDto dto, int userId)
    {
        var wallet = await _walletRepository.GetOneAsync(w => w.Id == dto.WalletId && w.User!.Id == userId, 
                         w => w.User!)
            ?? throw new WalletNotFoundException();

        var walletLimit = new WalletLimit
        {
            Wallet = wallet,
            LimitAmount = dto.LimitAmount,
            Month = new DateTime(dto.Month.Year, dto.Month.Month, 15).ToUniversalTime()
        };
        
        _walletRepository.Attach(wallet);
        
        await _walletLimitRepository.CreateAsync(walletLimit);
        await _walletLimitRepository.SaveChangesAsync();
    }

    public async Task UpdateWalletLimitAsync(int id, UpdateWalletLimitDto dto, int userId)
    {
        var walletLimit = await _walletLimitRepository.GetOneAsync(wl => wl.Id == id && wl.Wallet!.User!.Id == userId, 
                              wl => wl.Wallet!.User!)
            ?? throw new WalletLimitNotFoundException();
        
        walletLimit.LimitAmount = dto.LimitAmount;
        walletLimit.UpdatedAt = DateTimeOffset.UtcNow;
        
        _walletRepository.Attach(walletLimit.Wallet!);

        await _walletLimitRepository.UpdateAsync(walletLimit.Id, walletLimit);
        await _walletLimitRepository.SaveChangesAsync();
    }

    public async Task DeleteWalletLimitAsync(int id, int userId)
    {
        var walletLimit = await _walletLimitRepository.GetOneAsync(wl => wl.Id == id && wl.Wallet!.User!.Id == userId, wl => wl.Wallet!.User!)
            ?? throw new WalletLimitNotFoundException();

        await _walletLimitRepository.SoftDeleteAsync(walletLimit.Id);
        await _walletLimitRepository.SaveChangesAsync();
    }
}