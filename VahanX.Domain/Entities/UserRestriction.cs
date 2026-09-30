using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Controlled user restriction/suspension mechanism.
/// </summary>
public class UserRestriction : BaseEntity
{
    public Guid UserId { get; set; }
    public UserRestrictionType RestrictionType { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid CreatedByUserId { get; set; }
}
