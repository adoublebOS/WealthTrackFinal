using Microsoft.AspNetCore.Identity;
using WealthTrack.Domain.Common;

namespace WealthTrack.Domain.Entities;

public class User : IdentityUser<int>, IBaseEntity
{
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}