namespace VahanX.Domain.Enums;

/// <summary>
/// EV charging connector type.
/// </summary>
public enum ChargingConnectorType
{
    Type2 = 0,
    CCS2 = 1,
    CHAdeMO = 2,
    GBTAC = 3,
    GBTDC = 4,
    NACS = 5,
    Other = 99
}
