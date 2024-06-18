using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WealthTrack.Domain.Entities;
using WealthTrack.Infrastructure.Data;
using WealthTrack.Infrastructure.Repository;

namespace WealthTrack.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MainDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PgSQL")));
        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Card>, Repository<Card>>();
        services.AddScoped<IRepository<Category>, Repository<Category>>();
        services.AddScoped<IRepository<Transaction>, Repository<Transaction>>();
        services.AddScoped<IRepository<Wallet>, Repository<Wallet>>();
        services.AddScoped<IRepository<WalletLimit>, Repository<WalletLimit>>();
        services.AddScoped<IRepository<User>, Repository<User>>();
        services.AddScoped<IRepository<SavingsPlan>, Repository<SavingsPlan>>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        
        return services;
    }
}