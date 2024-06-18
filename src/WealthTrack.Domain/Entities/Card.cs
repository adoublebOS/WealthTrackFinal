using System.ComponentModel.DataAnnotations;
using WealthTrack.Domain.Common;

namespace WealthTrack.Domain.Entities;

public class Card : IBaseEntity
{
    public int Id { get; set; }
    [Required, MinLength(10), MaxLength(20)]
    public string? CardNumber { get; set; }
    [MaxLength(20)]
    public string? CardType { get; set; }
    public User? User { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}