using System.Linq.Expressions;
using WealthTrack.Domain.Entities;

namespace WealthTrack.Infrastructure.Repository;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllByFilterAsync(Expression<Func<Transaction, bool>> filter, params Expression<Func<Transaction, object>>[] includeProperties);
    Task<IEnumerable<Transaction>> GetAllByCategoriesAsync(List<int> categoriesIds, int userId);
    Task<IEnumerable<Transaction>> GetAllByWalletsAsync(List<int> walletsIds, int userId);
    Task CreateTransactionAsync(Transaction transaction, int userId, int walletId, int categoryId, int? savingsPlanId);
    Task UpdateTransactionAsync(int id, Transaction transaction, int userId, int walletId, int categoryId, int? savingsPlanId);
    Task DeleteTransactionAsync(int id, int userId);
}