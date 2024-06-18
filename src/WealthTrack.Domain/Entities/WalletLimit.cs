using WealthTrack.Domain.Attributes;
using WealthTrack.Domain.Common;

namespace WealthTrack.Domain.Entities;

public class WalletLimit : IBaseEntity
{
    public int Id { get; set; }
    public Wallet? Wallet { get; set; }
    [NonNegative]
    public decimal LimitAmount { get; set; }
    public DateTime Month { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}