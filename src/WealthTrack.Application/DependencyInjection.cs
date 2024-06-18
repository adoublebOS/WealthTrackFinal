using Microsoft.Extensions.DependencyInjection;
using WealthTrack.Application.Services.CardS;
using WealthTrack.Application.Services.CategoryS;
using WealthTrack.Application.Services.SavingsPlanS;
using WealthTrack.Application.Services.TransactionS;
using WealthTrack.Application.Services.WalletS;

namespace WealthTrack.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<ISavingsPlanService, SavingsPlanService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICardService, CardService>();
        
        return services;
    }
}