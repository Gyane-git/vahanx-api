namespace VahanX.Domain.Exceptions;

/// <summary>
/// Exception thrown when a conflict occurs (e.g., duplicate data).
/// </summary>
public class ConflictException : DomainException
{
    public ConflictException(string message)
        : base(message, "CONFLICT")
    {
    }
}
