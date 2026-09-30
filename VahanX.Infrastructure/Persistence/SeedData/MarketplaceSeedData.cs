using Microsoft.EntityFrameworkCore;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.SeedData;

/// <summary>
/// Seed data for marketplace module.
/// </summary>
public static class MarketplaceSeedData
{
    public static async Task SeedAsync(VahanXDbContext context)
    {
        await SeedLocations(context);
    }

    private static async Task SeedLocations(VahanXDbContext context)
    {
        if (await context.Locations.AnyAsync())
            return;

        var locations = new List<Location>
        {
            new()
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                Name = "Kathmandu",
                Province = "Bagmati",
                District = "Kathmandu",
                Municipality = "Kathmandu Metropolitan City",
                Ward = "1",
                StreetAddress = "Thamel",
                Latitude = 27.7172,
                Longitude = 85.3240
            },
            new()
            {
                Id = Guid.Parse("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                Name = "Lalitpur",
                Province = "Bagmati",
                District = "Lalitpur",
                Municipality = "Lalitpur Metropolitan City",
                Ward = "1",
                StreetAddress = "Pulchowk",
                Latitude = 27.6588,
                Longitude = 85.3247
            },
            new()
            {
                Id = Guid.Parse("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                Name = "Bhaktapur",
                Province = "Bagmati",
                District = "Bhaktapur",
                Municipality = "Bhaktapur Municipality",
                Ward = "1",
                StreetAddress = "Durbar Square",
                Latitude = 27.6710,
                Longitude = 85.4297
            },
            new()
            {
                Id = Guid.Parse("d4e5f6a7-b8c9-0123-defa-234567890123"),
                Name = "Pokhara",
                Province = "Gandaki",
                District = "Kaski",
                Municipality = "Pokhara Metropolitan City",
                Ward = "1",
                StreetAddress = "Lakeside",
                Latitude = 28.2096,
                Longitude = 83.9856
            },
            new()
            {
                Id = Guid.Parse("e5f6a7b8-c9d0-1234-efab-345678901234"),
                Name = "Chitwan",
                Province = "Bagmati",
                District = "Chitwan",
                Municipality = "Bharatpur Metropolitan City",
                Ward = "1",
                StreetAddress = "Narayangarh",
                Latitude = 27.5291,
                Longitude = 84.3542
            }
        };

        await context.Locations.AddRangeAsync(locations);
        await context.SaveChangesAsync();
    }
}
