using System.ComponentModel.DataAnnotations;
using WealthTrack.Domain.Attributes;
using WealthTrack.Domain.Common;

namespace WealthTrack.Domain.Entities;

public class Transaction : IBaseEntity
{
    public int Id { get; set; }
    [NonNegative]
    public decimal Amount { get; set; }
    public DateTimeOffset TransactionDate { get; set; } = DateTimeOffset.UtcNow;
    [MaxLength(255)]
    public string? Description { get; set; }
    
    public Wallet? Wallet { get; set; }
    public Category? Category { get; set; }
    public User? User { get; set; }
    public SavingsPlan? SavingsPlan { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}