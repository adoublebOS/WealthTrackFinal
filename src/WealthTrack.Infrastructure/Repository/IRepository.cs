using System.Linq.Expressions;

namespace WealthTrack.Infrastructure.Repository;

public interface IRepository<T>
{
    Task CreateAsync(T entity);
    Task SoftDeleteAsync(int id);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, T entity);
    Task<T?> GetOneAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties);
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
    Task<IEnumerable<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties);
    Task SaveChangesAsync();
    void Attach(T entity);
    void Detach(T entity);
}