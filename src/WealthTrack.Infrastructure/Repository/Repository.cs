using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WealthTrack.Domain.Common;
using WealthTrack.Infrastructure.Data;

namespace WealthTrack.Infrastructure.Repository;

public class Repository<T> : IRepository<T> where T : class, IBaseEntity
{
    private readonly MainDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(MainDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    
    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task SoftDeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            entity.DeletedAt = DateTimeOffset.UtcNow;
            _dbSet.Update(entity);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public async Task UpdateAsync(int id, T entity)
    {
        var existingEntity = await _dbSet.FindAsync(id);
        if (existingEntity != null)
        {
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            _dbSet.Update(existingEntity);
        }
    }

    public async Task<T?> GetOneAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet;
        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }
        return await query.AsNoTracking()
            .Where(t => t.DeletedAt == null)
            .FirstOrDefaultAsync(filter);
    }

    public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet;
        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }
        return await query
            .AsNoTracking()
            .Where(t => t.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet;
        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }
        return await query
            .AsNoTracking()            
            .Where(t => t.DeletedAt == null)
            .Where(filter)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Attach(T entity)
    {
        _dbSet.Attach(entity);
    }

    public void Detach(T entity)
    {
        _context.Entry(entity).State = EntityState.Detached;
    }
}