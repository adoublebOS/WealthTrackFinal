using WealthTrack.Application.DTO;
using WealthTrack.Domain.Entities;
using WealthTrack.Domain.Exceptions;
using WealthTrack.Infrastructure.Repository;

namespace WealthTrack.Application.Services.TransactionS;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    
    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<TransactionDto>> GetAllAsync(int userId)
    {
        var transactions = await _transactionRepository
            .GetAllByFilterAsync(
                t => t.User!.Id == userId,
                t => t.User!,
                    t => t.Wallet!,
                    t => t.Category,
                t => t.SavingsPlan);

        return transactions
            .OrderByDescending(t => t.TransactionDate)
            .Select(t => t.AsDto())
            .ToList();
    }

    public async Task<IEnumerable<TransactionDto>> GetAllByCategoriesAsync(List<int> categoriesIds, int userId)
    {
        var transactions = await _transactionRepository.GetAllByCategoriesAsync(categoriesIds, userId);
        return transactions
            .OrderByDescending(t => t.TransactionDate)
            .Select(t => t.AsDto())
            .ToList();
    }

    public async Task<IEnumerable<TransactionDto>> GetAllByWalletsAsync(List<int> walletsIds, int userId)
    {
        var transactions = await _transactionRepository.GetAllByWalletsAsync(walletsIds, userId);
        return transactions
            .OrderByDescending(t => t.TransactionDate)
            .Select(t => t.AsDto())
            .ToList();
    }

    public async Task CreateTransactionAsync(CreateTransactionDto dto, int userId)
    {
        if (dto.Amount <= 0)
        {
            throw new TransactionInvalidAmount();
        }
        
        var transaction = dto.AsEntity();

        await _transactionRepository.CreateTransactionAsync(transaction, userId, dto.WalletId, dto.CategoryId, dto.SavingsPlanId);
    }

    public async Task UpdateTransactionAsync(int id, UpdateTransactionDto dto, int userId)
    {
        if (dto.Amount <= 0)
        {
            throw new TransactionInvalidAmount();
        }
        
        var transaction = dto.AsEntity();

        await _transactionRepository.UpdateTransactionAsync(id, transaction, userId, dto.WalletId, dto.CategoryId, dto.SavingsPlanId);
    }

    public async Task DeleteTransactionAsync(int id, int userId)
    {
        await _transactionRepository.DeleteTransactionAsync(id, userId);
    }
}