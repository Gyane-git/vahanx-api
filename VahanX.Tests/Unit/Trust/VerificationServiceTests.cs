using Microsoft.EntityFrameworkCore;
using VahanX.Application.DTOs.Trust;
using VahanX.Application.Services;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;
using VahanX.Infrastructure.Persistence;
using VahanX.Infrastructure.Persistence.Repositories;
using Xunit;

namespace VahanX.Tests.Unit.Trust;

/// <summary>
/// Unit tests for VerificationService.
/// </summary>
public class VerificationServiceTests : IDisposable
{
    private readonly VahanXDbContext _context;
    private readonly VerificationService _service;

    public VerificationServiceTests()
    {
        var options = new DbContextOptionsBuilder<VahanXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new VahanXDbContext(options);

        var verificationRepository = new Repository<VehicleVerification>(_context);
        var vehicleRepository = new Repository<Vehicle>(_context);
        var listingRepository = new Repository<VehicleListing>(_context);
        var historyRepository = new Repository<VehicleVerificationStatusHistory>(_context);

        _service = new VerificationService(verificationRepository, vehicleRepository, listingRepository, historyRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesVerificationInPendingStatus()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            VariantId = Guid.NewGuid(),
            BodyTypeId = Guid.NewGuid(),
            FuelTypeId = Guid.NewGuid(),
            TransmissionTypeId = Guid.NewGuid(),
            DriveTypeId = Guid.NewGuid(),
            EngineTypeId = Guid.NewGuid()
        };
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();

        var request = new CreateVerificationRequest
        {
            VehicleId = vehicle.Id,
            VerificationType = VerificationType.BasicDocumentVerification
        };

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(VehicleVerificationStatus.Pending, result.Status);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidVehicleId_ThrowsNotFoundException()
    {
        // Arrange
        var request = new CreateVerificationRequest
        {
            VehicleId = Guid.NewGuid(),
            VerificationType = VerificationType.BasicDocumentVerification
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task SubmitAsync_WithPendingStatus_ChangesToInReview()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            VariantId = Guid.NewGuid(),
            BodyTypeId = Guid.NewGuid(),
            FuelTypeId = Guid.NewGuid(),
            TransmissionTypeId = Guid.NewGuid(),
            DriveTypeId = Guid.NewGuid(),
            EngineTypeId = Guid.NewGuid()
        };
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();

        var verification = new VehicleVerification
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Status = VehicleVerificationStatus.Pending,
            VerificationType = VerificationType.BasicDocumentVerification
        };
        await _context.VehicleVerifications.AddAsync(verification);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.SubmitAsync(verification.Id);

        // Assert
        Assert.Equal(VehicleVerificationStatus.InReview, result.Status);
    }

    [Fact]
    public async Task ApproveAsync_WithInReviewStatus_ChangesToVerified()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            VariantId = Guid.NewGuid(),
            BodyTypeId = Guid.NewGuid(),
            FuelTypeId = Guid.NewGuid(),
            TransmissionTypeId = Guid.NewGuid(),
            DriveTypeId = Guid.NewGuid(),
            EngineTypeId = Guid.NewGuid()
        };
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();

        var verification = new VehicleVerification
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Status = VehicleVerificationStatus.InReview,
            VerificationType = VerificationType.BasicDocumentVerification
        };
        await _context.VehicleVerifications.AddAsync(verification);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.ApproveAsync(verification.Id);

        // Assert
        Assert.Equal(VehicleVerificationStatus.Verified, result.Status);
        Assert.NotNull(result.VerifiedAt);
    }

    [Fact]
    public async Task RejectAsync_WithInReviewStatus_ChangesToRejected()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            VariantId = Guid.NewGuid(),
            BodyTypeId = Guid.NewGuid(),
            FuelTypeId = Guid.NewGuid(),
            TransmissionTypeId = Guid.NewGuid(),
            DriveTypeId = Guid.NewGuid(),
            EngineTypeId = Guid.NewGuid()
        };
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();

        var verification = new VehicleVerification
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Status = VehicleVerificationStatus.InReview,
            VerificationType = VerificationType.BasicDocumentVerification
        };
        await _context.VehicleVerifications.AddAsync(verification);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.RejectAsync(verification.Id, "Documents are unclear");

        // Assert
        Assert.Equal(VehicleVerificationStatus.Rejected, result.Status);
    }

    [Fact]
    public async Task RevokeAsync_WithVerifiedStatus_ChangesToRevoked()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            VariantId = Guid.NewGuid(),
            BodyTypeId = Guid.NewGuid(),
            FuelTypeId = Guid.NewGuid(),
            TransmissionTypeId = Guid.NewGuid(),
            DriveTypeId = Guid.NewGuid(),
            EngineTypeId = Guid.NewGuid()
        };
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();

        var verification = new VehicleVerification
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Status = VehicleVerificationStatus.Verified,
            VerificationType = VerificationType.BasicDocumentVerification,
            VerifiedAt = DateTime.UtcNow
        };
        await _context.VehicleVerifications.AddAsync(verification);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.RevokeAsync(verification.Id, "Fraud detected");

        // Assert
        Assert.Equal(VehicleVerificationStatus.Revoked, result.Status);
    }

    [Fact]
    public async Task GetPublicByVehicleIdAsync_WithVerifiedVerification_ReturnsPublicInfo()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            VariantId = Guid.NewGuid(),
            BodyTypeId = Guid.NewGuid(),
            FuelTypeId = Guid.NewGuid(),
            TransmissionTypeId = Guid.NewGuid(),
            DriveTypeId = Guid.NewGuid(),
            EngineTypeId = Guid.NewGuid()
        };
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();

        var verification = new VehicleVerification
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Status = VehicleVerificationStatus.Verified,
            VerificationType = VerificationType.BasicDocumentVerification,
            VerifiedAt = DateTime.UtcNow,
            VerificationReference = "VER-12345"
        };
        await _context.VehicleVerifications.AddAsync(verification);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetPublicByVehicleIdAsync(vehicle.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(VehicleVerificationStatus.Verified, result.Status);
        Assert.Equal("VER-12345", result.VerificationReference);
        Assert.True(result.IsVerified);
    }
}
