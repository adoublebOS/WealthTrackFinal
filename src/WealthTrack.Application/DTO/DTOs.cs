using WealthTrack.Domain.Entities;

namespace WealthTrack.Application.DTO;

#region Transaction

public record TransactionDto(
    int Id, 
    decimal Amount, 
    string? Description,
    DateTimeOffset TransactionDate, 
    string WalletName,
    string? CategoryName,
    string? SavingsName
);

public record CreateTransactionDto(
    decimal Amount,
    int WalletId,
    int CategoryId,
    string? Description,
    DateTimeOffset? TransactionDate,
    int? SavingsPlanId);

public record UpdateTransactionDto(
    decimal Amount,
    int WalletId,
    int CategoryId,
    string? Description,
    int? SavingsPlanId,
    DateTimeOffset? TransactionDate
);


#endregion

#region Wallet

public record WalletDto(
    int Id, 
    string Name, 
    decimal TotalAmount
);

public record CreateWalletDto(
    string Name,
    decimal TotalAmount
);

public record UpdateWalletDto(
    string Name,
    decimal TotalAmount
);
#endregion

#region WalletLimit

public record WalletLimitDto(
    int Id, 
    decimal LimitAmount, 
    DateTime Month
);

public record CreateWalletLimitDto(
    int WalletId, 
    decimal LimitAmount, 
    DateTime Month
);

public record UpdateWalletLimitDto(
    decimal LimitAmount
);


#endregion

#region Category

public record CategoryDto(
    int Id, 
    string Name, 
    string? Description
);

public record CreateCategoryDto(
    string Name,
    string? Description
);

public record UpdateCategoryDto(
    string Name,
    string? Description
);

#endregion

#region User

public record UserDto(
    int Id, 
    string UserName, 
    string Email, 
    string? PhoneNumber
);

#endregion

#region Card

public record CardDto(
    int Id, 
    string CardNumber, 
    string? CardType, 
    DateTime ExpirationDate
);

public record CreateCardDto(
    string CardNumber,
    string? CardType,
    DateTime ExpirationDate
);

public record UpdateCardDto(
    string CardNumber,
    string? CardType,
    DateTime ExpirationDate
);

#endregion

#region SavingsPlan

public record SavingsPlanDto(
    int Id,
    string Title,
    string? Description,
    decimal Balance,
    decimal Goal,
    DateTime GoalDate
);

public record CreateSavingsPlanDto(
    string Title,
    string? Description,
    decimal Balance,
    decimal Goal,
    DateTime GoalDate
);

public record UpdateSavingsPlanDto(
    string Title,
    string? Description,
    decimal Balance,
    decimal Goal,
    DateTime GoalDate
);
#endregion