namespace VahanX.Domain.Exceptions;

/// <summary>
/// Exception thrown when access is forbidden.
/// </summary>
public class ForbiddenException : DomainException
{
    public ForbiddenException(string message)
        : base(message, "FORBIDDEN")
    {
    }
}
