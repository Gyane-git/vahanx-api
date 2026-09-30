using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Trust;

/// <summary>
/// Ownership history response DTO.
/// </summary>
public class OwnershipHistoryResponse
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public int OwnerSequence { get; set; }
    public OwnershipType OwnershipType { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public HistorySource Source { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Mileage history response DTO.
/// </summary>
public class MileageHistoryResponse
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public int Mileage { get; set; }
    public string MileageUnit { get; set; } = string.Empty;
    public DateTime RecordedAt { get; set; }
    public HistorySource Source { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Service history response DTO.
/// </summary>
public class ServiceHistoryResponse
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime ServiceDate { get; set; }
    public int? Mileage { get; set; }
    public ServiceType ServiceType { get; set; }
    public string? ServiceProvider { get; set; }
    public string? Description { get; set; }
    public decimal? Cost { get; set; }
    public HistorySource Source { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Accident history response DTO.
/// </summary>
public class AccidentHistoryResponse
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime? AccidentDate { get; set; }
    public AccidentSeverity Severity { get; set; }
    public string? Description { get; set; }
    public RepairStatus RepairStatus { get; set; }
    public decimal? RepairCost { get; set; }
    public HistorySource Source { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Insurance history response DTO.
/// </summary>
public class InsuranceHistoryResponse
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public string? Provider { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public InsuranceStatus Status { get; set; }
    public HistorySource Source { get; set; }
}

/// <summary>
/// Registration history response DTO.
/// </summary>
public class RegistrationHistoryResponse
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public RegistrationStatus RegistrationStatus { get; set; }
    public string? RegistrationArea { get; set; }
    public HistorySource Source { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Price history response DTO.
/// </summary>
public class PriceHistoryResponse
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
    public string? Reason { get; set; }
}

/// <summary>
/// Aggregated vehicle history response DTO.
/// </summary>
public class VehicleHistoryResponse
{
    public Guid VehicleId { get; set; }
    public int TotalPreviousOwners { get; set; }
    public List<OwnershipHistoryResponse> OwnershipHistory { get; set; } = [];
    public List<MileageHistoryResponse> MileageHistory { get; set; } = [];
    public List<ServiceHistoryResponse> ServiceHistory { get; set; } = [];
    public List<AccidentHistoryResponse> AccidentHistory { get; set; } = [];
    public List<InsuranceHistoryResponse> InsuranceHistory { get; set; } = [];
    public List<RegistrationHistoryResponse> RegistrationHistory { get; set; } = [];
}
