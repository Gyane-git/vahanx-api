using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Canonical vehicle definition.
/// This is NOT a marketplace listing.
/// </summary>
public class Vehicle : BaseEntity
{
    public Guid VariantId { get; set; }

    public Variant? Variant { get; set; }

    public Guid BodyTypeId { get; set; }

    public BodyType? BodyType { get; set; }

    public Guid FuelTypeId { get; set; }

    public FuelType? FuelType { get; set; }

    public Guid TransmissionTypeId { get; set; }

    public TransmissionType? TransmissionType { get; set; }

    public Guid DriveTypeId { get; set; }

    public DriveType? DriveType { get; set; }

    public Guid EngineTypeId { get; set; }

    public EngineType? EngineType { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
