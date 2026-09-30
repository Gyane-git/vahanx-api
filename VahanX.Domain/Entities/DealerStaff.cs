using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Staff member of a dealer.
/// Links to a User for future authorization integration.
/// </summary>
public class DealerStaff : BaseEntity
{
    public Guid DealerId { get; set; }

    public Dealer? Dealer { get; set; }

    public Guid UserId { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Role { get; set; }

    public bool IsActive { get; set; } = true;
}
