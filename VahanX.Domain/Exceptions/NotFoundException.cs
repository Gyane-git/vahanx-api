namespace VahanX.Domain.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found.
/// </summary>
public class NotFoundException : DomainException
{
    public NotFoundException(string message)
        : base(message, "NOT_FOUND")
    {
    }

    public NotFoundException(string entityName, object key)
        : base($"Entity \"{entityName}\" with key \"{key}\" was not found.", "NOT_FOUND")
    {
    }
}
