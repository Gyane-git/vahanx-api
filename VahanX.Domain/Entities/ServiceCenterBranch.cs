using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Physical branch/location of a service center.
/// </summary>
public class ServiceCenterBranch : BaseEntity
{
    public Guid ServiceCenterId { get; set; }

    public ServiceCenter? ServiceCenter { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid? LocationId { get; set; }

    public Location? Location { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string? ContactPhone { get; set; }

    public string? ContactEmail { get; set; }

    public ServiceCenterStatus Status { get; set; } = ServiceCenterStatus.Active;

    public ICollection<ServiceCenterService> Services { get; set; } = new List<ServiceCenterService>();

    public ICollection<ServicePackage> Packages { get; set; } = new List<ServicePackage>();

    public ICollection<ServiceWorkingHour> WorkingHours { get; set; } = new List<ServiceWorkingHour>();

    public ICollection<ServiceBooking> Bookings { get; set; } = new List<ServiceBooking>();
}
