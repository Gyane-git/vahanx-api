using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Refresh token record. Only a SHA-256 hash of the token is stored.
/// </summary>
public class RefreshToken : BaseEntity
{
    public Guid UserId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? ReplacedByTokenHash { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }

    public bool IsActive => RevokedAt is null && ExpiresAt > DateTime.UtcNow && !IsDeleted;
}
