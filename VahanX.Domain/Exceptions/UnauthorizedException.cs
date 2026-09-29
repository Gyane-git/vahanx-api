namespace VahanX.Domain.Exceptions;

/// <summary>
/// Exception thrown when authentication is required or fails.
/// </summary>
public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string message)
        : base(message, "UNAUTHORIZED")
    {
    }
}
