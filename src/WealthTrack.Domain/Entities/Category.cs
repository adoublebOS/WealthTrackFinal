using System.ComponentModel.DataAnnotations;
using WealthTrack.Domain.Common;

namespace WealthTrack.Domain.Entities;

public class Category : IBaseEntity
{
    public int Id { get; set; }
    [Required, MinLength(5), MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(255)]
    public string? Description { get; set; }
    public User? User { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}