using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Engagement;

/// <summary>
/// Enquiry response DTO.
/// </summary>
public class EnquiryResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ListingId { get; set; }
    public string ListingTitle { get; set; } = string.Empty;
    public Guid? SellerId { get; set; }
    public string? SellerName { get; set; }
    public Guid? DealerId { get; set; }
    public string? DealerName { get; set; }
    public EnquiryType EnquiryType { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public EnquiryStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
}

/// <summary>
/// Create enquiry request.
/// </summary>
public class CreateEnquiryRequest
{
    public Guid ListingId { get; set; }
    public EnquiryType EnquiryType { get; set; } = EnquiryType.General;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Update enquiry request.
/// </summary>
public class UpdateEnquiryRequest
{
    public string? Subject { get; set; }
    public string? Message { get; set; }
}

/// <summary>
/// Respond to enquiry request.
/// </summary>
public class RespondEnquiryRequest
{
    public string Response { get; set; } = string.Empty;
}
