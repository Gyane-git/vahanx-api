namespace VahanX.Application.Common;

/// <summary>
/// Strongly typed JWT configuration. Bound from the "Jwt" configuration section.
/// </summary>
public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "VahanX";
    public string Audience { get; set; } = "VahanX.Admin";
    public string SigningKey { get; set; } = string.Empty;
    public int AccessTokenExpirationMinutes { get; set; } = 30;
    public int RefreshTokenExpirationDays { get; set; } = 7;
}

/// <summary>
/// Password policy configuration. Bound from the "PasswordPolicy" configuration section.
/// </summary>
public class PasswordPolicySettings
{
    public const string SectionName = "PasswordPolicy";

    public int MinimumLength { get; set; } = 8;
    public bool RequireUppercase { get; set; } = true;
    public bool RequireLowercase { get; set; } = true;
    public bool RequireDigit { get; set; } = true;
    public bool RequireNonAlphanumeric { get; set; } = true;
}
