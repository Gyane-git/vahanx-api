using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Working hours for a service center branch.
/// </summary>
public class ServiceWorkingHour : BaseEntity
{
    public Guid ServiceCenterBranchId { get; set; }

    public ServiceCenterBranch? ServiceCenterBranch { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeSpan OpeningTime { get; set; }

    public TimeSpan ClosingTime { get; set; }

    public bool IsClosed { get; set; }
}
