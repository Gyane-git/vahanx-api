namespace VahanX.Domain.Enums;

/// <summary>
/// Type of verification performed.
/// </summary>
public enum VerificationType
{
    BasicDocumentVerification = 0,
    VehicleIdentityVerification = 1,
    OwnershipVerification = 2,
    RegistrationVerification = 3,
    InsuranceVerification = 4,
    InspectionBasedVerification = 5
}
