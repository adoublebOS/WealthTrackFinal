using System.ComponentModel.DataAnnotations;
using WealthTrack.Domain.Common;

namespace WealthTrack.Domain.Entities;

public class SavingsPlan : IBaseEntity
{
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string? Title { get; set; }
    [MaxLength(255)]
    public string? Description { get; set; }
    public decimal Balance { get; set; }
    public decimal Goal { get; set; }
    public DateTime GoalDate { get; set; }
    public User? User { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}