using WealthTrack.Domain.Entities;
using WealthTrack.Infrastructure.Repository;

namespace WealthTrack.Application.DTO;

public static class Extensions
{
    #region Transaction

    public static TransactionDto AsDto(this Transaction transaction)
    {
        return new TransactionDto(
            transaction.Id,
            transaction.Amount,
            transaction.Description,
            transaction.TransactionDate.ToLocalTime(),
            transaction.Wallet!.Name!,
            transaction.Category?.Name,
            transaction.SavingsPlan?.Title
        );
    }

    public static Transaction AsEntity(this CreateTransactionDto dto)
    {
        return new Transaction
        {
            Amount = dto.Amount,
            Wallet = null,
            Category = null,
            User = null,
            Description = dto.Description,
            TransactionDate = dto.TransactionDate ?? DateTimeOffset.UtcNow,
            SavingsPlan = null
        };
    }
    
    public static Transaction AsEntity(this CreateTransactionDto dto, 
        Wallet wallet, 
        Category category, 
        User user,
        SavingsPlan? savingsPlan)
    {
        return new Transaction
        {
            Amount = dto.Amount,
            Wallet = wallet,
            Category = category,
            User = user,
            Description = dto.Description,
            TransactionDate = dto.TransactionDate ?? DateTimeOffset.UtcNow,
            SavingsPlan = savingsPlan
        };
    }

    public static void UpdateEntity(this UpdateTransactionDto dto, Transaction transaction)
    {
        transaction.Amount = dto.Amount;
        transaction.Wallet = transaction.Wallet;
        transaction.Category = transaction.Category;
        transaction.Description = dto.Description;
        transaction.TransactionDate = dto.TransactionDate ?? DateTimeOffset.UtcNow;
        transaction.SavingsPlan = transaction.SavingsPlan;
    }

    public static Transaction AsEntity(this UpdateTransactionDto dto)
    {
        return new Transaction
        {
            Amount = dto.Amount,
            Wallet = null,
            Category = null,
            User = null,
            Description = dto.Description,
            TransactionDate = dto.TransactionDate ?? DateTimeOffset.UtcNow,
            SavingsPlan = null
        };
    }
    #endregion
    
    #region Wallet

    public static WalletDto AsDto(this Wallet wallet)
    {
        return new WalletDto(
            wallet.Id,
            wallet.Name!,
            wallet.TotalAmount
        );
    }

    public static Wallet AsEntity(this CreateWalletDto dto, User user)
    {
        return new Wallet
        {
            Name = dto.Name,
            TotalAmount = dto.TotalAmount,
            User = user
        };
    }

    public static void UpdateEntity(this UpdateWalletDto dto, Wallet wallet)
    {
        wallet.Name = dto.Name;
        wallet.TotalAmount = dto.TotalAmount;
    }

    #endregion

    #region WalletLimit

    public static WalletLimitDto AsDto(this WalletLimit walletLimit)
    {
        return new WalletLimitDto(walletLimit.Id, walletLimit.LimitAmount, walletLimit.Month);
    }

    #endregion
    
    
    #region Category

    public static CategoryDto AsDto(this Category category)
    {
        return new CategoryDto(
            category.Id,
            category.Name!,
            category.Description
        );
    }

    public static Category AsEntity(this CreateCategoryDto dto, User user)
    {
        return new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            User = user
        };
    }

    public static void UpdateEntity(this UpdateCategoryDto dto, Category category)
    {
        category.Name = dto.Name;
        category.Description = dto.Description;
    }

    #endregion
    
    #region User

    public static UserDto AsDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.UserName!,
            user.Email!,
            user.PhoneNumber
        );
    }

    #endregion
    
    #region Card

    public static CardDto AsDto(this Card card)
    {
        return new CardDto(
            card.Id,
            card.CardNumber!,
            card.CardType,
            card.ExpirationDate.ToLocalTime()
        );
    }

    public static Card AsEntity(this CreateCardDto dto, User user)
    {
        return new Card
        {
            CardNumber = dto.CardNumber,
            CardType = dto.CardType,
            ExpirationDate = dto.ExpirationDate.ToUniversalTime(),
            User = user
        };
    }

    public static void UpdateEntity(this UpdateCardDto dto, Card card)
    {
        card.CardNumber = dto.CardNumber;
        card.CardType = dto.CardType;
        card.ExpirationDate = dto.ExpirationDate.ToUniversalTime();
    }

    #endregion

    #region SavingsPlan

    public static SavingsPlanDto AsDto(this SavingsPlan savingsPlan)
    {
        return new SavingsPlanDto(
            savingsPlan.Id,
            savingsPlan.Title!,
            savingsPlan.Description,
            savingsPlan.Balance,
            savingsPlan.Goal,
            savingsPlan.GoalDate.ToLocalTime()
        );
    }

    public static SavingsPlan AsEntity(this CreateSavingsPlanDto dto, User user)
    {
        return new SavingsPlan
        {
            Title = dto.Title,
            Description = dto.Description,
            Balance = dto.Balance,
            Goal = dto.Goal,
            GoalDate = dto.GoalDate.ToUniversalTime(),
            User = user
        };
    }

    public static void UpdateEntity(this UpdateSavingsPlanDto dto, SavingsPlan savingsPlan)
    {
        savingsPlan.Title = dto.Title;
        savingsPlan.Description = dto.Description;
        savingsPlan.Balance = dto.Balance;
        savingsPlan.Goal = dto.Goal;
        savingsPlan.GoalDate = dto.GoalDate.ToUniversalTime();
    }
    #endregion
}