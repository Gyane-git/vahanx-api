namespace VahanX.Domain.Exceptions;

/// <summary>
/// Exception thrown when a CMS page is not found.
/// </summary>
public class CmsPageNotFoundException : DomainException
{
    public CmsPageNotFoundException(string slug)
        : base($"CMS page with slug '{slug}' not found.", "CMS_PAGE_NOT_FOUND")
    {
    }
}

/// <summary>
/// Exception thrown when a duplicate slug is detected.
/// </summary>
public class DuplicateSlugException : DomainException
{
    public DuplicateSlugException(string slug, string entityType)
        : base($"Slug '{slug}' is already in use for {entityType}.", "DUPLICATE_SLUG")
    {
    }
}

/// <summary>
/// Exception thrown when a content target reference is invalid.
/// </summary>
public class InvalidContentTargetException : DomainException
{
    public InvalidContentTargetException(string message)
        : base(message, "INVALID_CONTENT_TARGET")
    {
    }
}

/// <summary>
/// Exception thrown when unauthorized publishing is attempted.
/// </summary>
public class UnauthorizedPublishingException : DomainException
{
    public UnauthorizedPublishingException(string message)
        : base(message, "UNAUTHORIZED_PUBLISHING")
    {
    }
}
