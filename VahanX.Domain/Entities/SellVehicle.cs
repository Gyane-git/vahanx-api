using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Vehicle submitted by an owner for selling.
/// Uses existing Vehicle Core master data references.
/// </summary>
public class SellVehicle : BaseEntity
{
    public Guid SellRequestId { get; set; }

    public SellRequest? SellRequest { get; set; }

    public Guid? VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    public Guid VehicleTypeId { get; set; }

    public VehicleType? VehicleType { get; set; }

    public Guid BrandId { get; set; }

    public Brand? Brand { get; set; }

    public Guid ModelId { get; set; }

    public Model? Model { get; set; }

    public Guid? VariantId { get; set; }

    public Variant? Variant { get; set; }

    public int ManufactureYear { get; set; }

    public int? RegistrationYear { get; set; }

    public int Mileage { get; set; }

    public string MileageUnit { get; set; } = "km";

    public Guid FuelTypeId { get; set; }

    public FuelType? FuelType { get; set; }

    public Guid? TransmissionTypeId { get; set; }

    public TransmissionType? TransmissionType { get; set; }

    public Condition Condition { get; set; } = Condition.Used;

    public decimal? AskingPrice { get; set; }

    public string? Description { get; set; }

    public Guid? LocationId { get; set; }

    public Location? Location { get; set; }
}
