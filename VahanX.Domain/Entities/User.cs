using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// User entity for the VahanX platform.
/// </summary>
public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool EmailConfirmed { get; set; }
    public DateTime? LastLoginAt { get; set; }

    /// <summary>PBKDF2 password hash produced by ASP.NET Core PasswordHasher. Never store plaintext.</summary>
    public string PasswordHash { get; set; } = string.Empty;
}
