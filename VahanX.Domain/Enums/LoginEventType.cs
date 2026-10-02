namespace VahanX.Domain.Enums;

/// <summary>
/// Types of authentication events recorded in login history.
/// </summary>
public enum LoginEventType
{
    Login = 1,
    Logout = 2,
    Refresh = 3,
    LoginFailed = 4,
    AccountBlocked = 5
}
