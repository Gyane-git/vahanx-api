using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Join entity between User and Role.
/// </summary>
public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
