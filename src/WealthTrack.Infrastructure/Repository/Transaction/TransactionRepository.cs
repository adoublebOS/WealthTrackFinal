using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WealthTrack.Domain.Entities;
using WealthTrack.Domain.Exceptions;
using WealthTrack.Infrastructure.Data;

namespace WealthTrack.Infrastructure.Repository;

public class TransactionRepository : ITransactionRepository
{
    private readonly MainDbContext _context;
    private readonly DbSet<Transaction> _dbSet;
    
    public TransactionRepository(MainDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Transaction>();
    }


    public async Task<IEnumerable<Transaction>> GetAllByFilterAsync(Expression<Func<Transaction, bool>> filter, params Expression<Func<Transaction, object>>[] includeProperties)
    {
        IQueryable<Transaction> query = _dbSet;
        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }
        return await query
            .AsNoTracking()
            .Where(t => t.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetAllByCategoriesAsync(List<int> categoriesIds, int userId)
    {
        return await _dbSet
            .Include(t => t.User)
            .Include(t => t.Wallet)
            .Include(t => t.Category)
            .Where(t => t.User!.Id == userId && categoriesIds.Contains(t.Category.Id))
            .Where(t => t.DeletedAt == null)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetAllByWalletsAsync(List<int> walletsIds, int userId)
    {
        return await _dbSet
            .Include(t => t.User)
            .Include(t => t.Wallet)
            .Where(t => t.User!.Id == userId && walletsIds.Contains(t.Wallet!.Id))
            .Where(t => t.DeletedAt == null)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task CreateTransactionAsync(Transaction transaction, int userId, int walletId, int categoryId, int? savingsPlanId)
    {
        var user = await _context.Users            
                       .Where(u => u.DeletedAt == null)
                       .FirstOrDefaultAsync(u => u.Id == userId) 
                   ?? throw new UserNotFoundException();
        var wallet = await _context.Wallets
                         .Where(w => w.DeletedAt == null)
                         .FirstOrDefaultAsync(w => w.Id == walletId)
                     ?? throw new WalletNotFoundException();
        var category = await _context.Categories
                           .Where(c => c.DeletedAt == null)
                           .FirstOrDefaultAsync(c => c.Id == categoryId)
                       ?? throw new CategoryNotFoundException();

        var currentMonth = new DateTime(transaction.TransactionDate.Year, transaction.TransactionDate.Month, 15);
        var walletLimit = await _context.WalletLimits.FirstOrDefaultAsync(
            wl => wl.Wallet!.Id == wallet.Id && wl.Month.ToLocalTime() == currentMonth);
        
        if (walletLimit is not null && (walletLimit.LimitAmount - wallet.TotalAmount) < transaction.Amount)
        {
            throw new WalletLimitExceededException();
        }
        
        if (savingsPlanId is not null)
        {
            var savingsPlan = await _context.SavingsPlans
                                  .Where(sp => sp.DeletedAt == null)
                                  .Include(sp => sp.User)
                                  .FirstOrDefaultAsync(sp => sp.Id == savingsPlanId
                                  && sp.User!.Id == userId)
                              ?? throw new SavingsPlanNotFoundException();
            if (savingsPlan.User!.Id != userId) throw new AccessDeniedException();

            transaction.SavingsPlan = savingsPlan;
            
            savingsPlan.Balance += transaction.Amount;
            
            savingsPlan.UpdatedAt = DateTimeOffset.UtcNow;
            _context.SavingsPlans.Update(savingsPlan);
        }
        
        wallet.TotalAmount -= transaction.Amount;
        wallet.UpdatedAt = DateTimeOffset.UtcNow;
        _context.Wallets.Update(wallet);
        
        transaction.Wallet = wallet;
        transaction.User = user;
        transaction.Category = category;
        transaction.TransactionDate = transaction.TransactionDate.ToUniversalTime();
        
        await _dbSet.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTransactionAsync(int id, Transaction transaction, int userId, int walletId, int categoryId, int? savingsPlanId)
    {
        var existingTransaction = await _dbSet
                                      .Include(t => t.User)
                                      .Include(t => t.Wallet)
                                      .Include(t => t.Category)
                                      .Where(t => t.DeletedAt == null)
                                      .FirstOrDefaultAsync(t => t.Id == id && t.User!.Id == userId) ??
                                  throw new TransactionNotFoundException();

        if (existingTransaction.User!.Id != userId) throw new AccessDeniedException();

        existingTransaction.Wallet!.TotalAmount += existingTransaction.Amount;
        _context.Wallets.Update(existingTransaction.Wallet);
        
        existingTransaction.Amount = transaction.Amount;
        existingTransaction.TransactionDate = transaction.TransactionDate;
        existingTransaction.Description = transaction.Description;
        existingTransaction.UpdatedAt = DateTimeOffset.UtcNow;
        
        var user = await _context.Users            
                       .Where(u => u.DeletedAt == null)
                       .FirstOrDefaultAsync(u => u.Id == userId) 
                   ?? throw new UserNotFoundException();
        var wallet = await _context.Wallets
                         .Where(w => w.DeletedAt == null)
                         .FirstOrDefaultAsync(w => w.Id == walletId)
                     ?? throw new WalletNotFoundException();
        var category = await _context.Categories
                           .Where(c => c.DeletedAt == null)
                           .FirstOrDefaultAsync(c => c.Id == categoryId)
                       ?? throw new CategoryNotFoundException();

        var currentMonth = new DateTime(transaction.TransactionDate.Year, transaction.TransactionDate.Month, 15);
        var walletLimit = await _context.WalletLimits.FirstOrDefaultAsync(
            wl => wl.Wallet!.Id == wallet.Id && wl.Month.ToLocalTime() == currentMonth);
        
        if (walletLimit is not null && (walletLimit.LimitAmount - wallet.TotalAmount) < transaction.Amount)
        {
            throw new WalletLimitExceededException();
        }
        
        existingTransaction.Wallet = wallet; 
        existingTransaction.Category = category;

        if (existingTransaction.SavingsPlan is not null)
        {
            var oldSavingsPlan = (await _context.SavingsPlans            
                .Where(sp => sp.DeletedAt == null)
                .FirstAsync(sp => sp.Id == existingTransaction.SavingsPlan.Id));
            oldSavingsPlan.Balance -= transaction.Amount;

            _context.SavingsPlans.Update(oldSavingsPlan);
        }

        if (savingsPlanId is not null)
        {
            var newSavingsPlan = await _context.SavingsPlans
                                     .Include(sp => sp.User)
                                     .Where(sp => sp.DeletedAt == null)
                                     .FirstOrDefaultAsync(sp => sp.Id == savingsPlanId) ??
                                 throw new SavingsPlanNotFoundException();

            if (newSavingsPlan.User!.Id != userId) throw new AccessDeniedException();

            newSavingsPlan.Balance += transaction.Amount;

            _context.SavingsPlans.Update(newSavingsPlan);
        }

        wallet.TotalAmount -= transaction.Amount;
        wallet.UpdatedAt = DateTimeOffset.UtcNow;
        
        _context.Entry(existingTransaction).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTransactionAsync(int id, int userId)
    {
        var transaction = await _dbSet
                              .Include(t => t.User)
                              .Include(t => t.Wallet)
                              .Include(t => t.Category)
                              .Include(t => t.SavingsPlan)
                              .Where(t => t.DeletedAt == null)
                              .FirstOrDefaultAsync(t => t.Id == id && t.User!.Id == userId) ??
                          throw new TransactionNotFoundException();

        if (transaction.User!.Id != userId) throw new AccessDeniedException();

        transaction.DeletedAt = DateTimeOffset.UtcNow;

        transaction.Wallet!.TotalAmount += transaction.Amount;
        if (transaction.SavingsPlan is not null)
        {
            transaction.SavingsPlan.Balance -= transaction.Amount;
            _context.Entry(transaction.SavingsPlan).State = EntityState.Modified;
        }
        
        _context.Entry(transaction.Wallet).State = EntityState.Modified;
        _context.Entry(transaction).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}