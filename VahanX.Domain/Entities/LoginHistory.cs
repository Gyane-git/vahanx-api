using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Login history / authentication event record.
/// </summary>
public class LoginHistory : BaseEntity
{
    public Guid? UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public LoginEventType EventType { get; set; }
    public bool Success { get; set; }
    public string? FailureReason { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime Timestamp { get; set; }
}
