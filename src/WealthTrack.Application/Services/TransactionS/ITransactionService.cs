using WealthTrack.Application.DTO;

namespace WealthTrack.Application.Services.TransactionS;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetAllAsync(int userId);
    Task<IEnumerable<TransactionDto>> GetAllByCategoriesAsync(List<int> categoriesIds, int userId);
    Task<IEnumerable<TransactionDto>> GetAllByWalletsAsync(List<int> walletsIds, int userId);
    Task CreateTransactionAsync(CreateTransactionDto dto, int userId);
    Task UpdateTransactionAsync(int id, UpdateTransactionDto dto, int userId);
    Task DeleteTransactionAsync(int id, int userId);
}
