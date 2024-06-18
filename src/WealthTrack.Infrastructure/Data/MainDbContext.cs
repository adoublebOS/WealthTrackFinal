using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WealthTrack.Domain.Entities;

namespace WealthTrack.Infrastructure.Data;

public class MainDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public MainDbContext(DbContextOptions<MainDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Card> Cards { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<SavingsPlan> SavingsPlans { get; set; }
    public DbSet<WalletLimit> WalletLimits { get; set; }
}