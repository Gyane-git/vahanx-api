using Microsoft.EntityFrameworkCore;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.SeedData;

/// <summary>
/// Seed data for Vehicle Core module.
/// Only minimal development seed data - no production business data.
/// </summary>
public static class VehicleCoreSeedData
{
    public static async Task SeedAsync(VahanXDbContext context, CancellationToken cancellationToken = default)
    {
        await SeedVehicleTypesAsync(context, cancellationToken);
        await SeedBodyTypesAsync(context, cancellationToken);
        await SeedFuelTypesAsync(context, cancellationToken);
        await SeedTransmissionTypesAsync(context, cancellationToken);
        await SeedDriveTypesAsync(context, cancellationToken);
        await SeedEngineTypesAsync(context, cancellationToken);
    }

    private static async Task SeedVehicleTypesAsync(VahanXDbContext context, CancellationToken cancellationToken)
    {
        if (await context.VehicleTypes.AnyAsync(cancellationToken))
            return;

        var vehicleTypes = new List<VehicleType>
        {
            new() { Name = "Car", Code = "CAR", Description = "Standard passenger car", DisplayOrder = 1 },
            new() { Name = "Bike", Code = "BIKE", Description = "Two-wheeler motorcycle", DisplayOrder = 2 },
            new() { Name = "Scooter", Code = "SCOOTER", Description = "Motor scooter", DisplayOrder = 3 },
            new() { Name = "SUV", Code = "SUV", Description = "Sport Utility Vehicle", DisplayOrder = 4 },
            new() { Name = "Bus", Code = "BUS", Description = "Passenger bus", DisplayOrder = 5 },
            new() { Name = "Truck", Code = "TRUCK", Description = "Commercial truck", DisplayOrder = 6 },
            new() { Name = "EV", Code = "EV", Description = "Electric Vehicle", DisplayOrder = 7 }
        };

        await context.VehicleTypes.AddRangeAsync(vehicleTypes, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedBodyTypesAsync(VahanXDbContext context, CancellationToken cancellationToken)
    {
        if (await context.BodyTypes.AnyAsync(cancellationToken))
            return;

        var bodyTypes = new List<BodyType>
        {
            new() { Name = "Sedan", Code = "SEDAN", Description = "Sedan body type", DisplayOrder = 1 },
            new() { Name = "Hatchback", Code = "HATCHBACK", Description = "Hatchback body type", DisplayOrder = 2 },
            new() { Name = "SUV", Code = "SUV", Description = "SUV body type", DisplayOrder = 3 },
            new() { Name = "Pickup", Code = "PICKUP", Description = "Pickup truck body type", DisplayOrder = 4 },
            new() { Name = "Van", Code = "VAN", Description = "Van body type", DisplayOrder = 5 }
        };

        await context.BodyTypes.AddRangeAsync(bodyTypes, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedFuelTypesAsync(VahanXDbContext context, CancellationToken cancellationToken)
    {
        if (await context.FuelTypes.AnyAsync(cancellationToken))
            return;

        var fuelTypes = new List<FuelType>
        {
            new() { Name = "Petrol", Code = "PETROL", Description = "Petrol fuel", DisplayOrder = 1 },
            new() { Name = "Diesel", Code = "DIESEL", Description = "Diesel fuel", DisplayOrder = 2 },
            new() { Name = "Electric", Code = "ELECTRIC", Description = "Electric power", DisplayOrder = 3 },
            new() { Name = "Hybrid", Code = "HYBRID", Description = "Hybrid power", DisplayOrder = 4 }
        };

        await context.FuelTypes.AddRangeAsync(fuelTypes, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedTransmissionTypesAsync(VahanXDbContext context, CancellationToken cancellationToken)
    {
        if (await context.TransmissionTypes.AnyAsync(cancellationToken))
            return;

        var transmissionTypes = new List<TransmissionType>
        {
            new() { Name = "Manual", Code = "MANUAL", Description = "Manual transmission", DisplayOrder = 1 },
            new() { Name = "Automatic", Code = "AUTOMATIC", Description = "Automatic transmission", DisplayOrder = 2 },
            new() { Name = "CVT", Code = "CVT", Description = "Continuously Variable Transmission", DisplayOrder = 3 }
        };

        await context.TransmissionTypes.AddRangeAsync(transmissionTypes, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedDriveTypesAsync(VahanXDbContext context, CancellationToken cancellationToken)
    {
        if (await context.DriveTypes.AnyAsync(cancellationToken))
            return;

        var driveTypes = new List<VahanX.Domain.Entities.DriveType>
        {
            new() { Name = "FWD", Code = "FWD", Description = "Front Wheel Drive", DisplayOrder = 1 },
            new() { Name = "RWD", Code = "RWD", Description = "Rear Wheel Drive", DisplayOrder = 2 },
            new() { Name = "AWD", Code = "AWD", Description = "All Wheel Drive", DisplayOrder = 3 },
            new() { Name = "4WD", Code = "4WD", Description = "Four Wheel Drive", DisplayOrder = 4 }
        };

        await context.DriveTypes.AddRangeAsync(driveTypes, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedEngineTypesAsync(VahanXDbContext context, CancellationToken cancellationToken)
    {
        if (await context.EngineTypes.AnyAsync(cancellationToken))
            return;

        var engineTypes = new List<EngineType>
        {
            new() { Name = "ICE", Code = "ICE", Description = "Internal Combustion Engine", DisplayOrder = 1 },
            new() { Name = "Electric Motor", Code = "ELECTRIC_MOTOR", Description = "Electric motor", DisplayOrder = 2 },
            new() { Name = "Hybrid", Code = "HYBRID", Description = "Hybrid engine", DisplayOrder = 3 },
            new() { Name = "Plug-in Hybrid", Code = "PLUGIN_HYBRID", Description = "Plug-in hybrid engine", DisplayOrder = 4 }
        };

        await context.EngineTypes.AddRangeAsync(engineTypes, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
