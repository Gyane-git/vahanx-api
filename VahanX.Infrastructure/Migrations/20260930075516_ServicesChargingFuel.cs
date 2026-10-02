using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VahanX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ServicesChargingFuel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChargingStations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    OperatorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VerificationStatus = table.Column<int>(type: "int", nullable: false),
                    Is24Hours = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargingStations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargingStations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "FuelStations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OperatorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Is24Hours = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VerificationStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelStations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelStations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCenters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    OwnerUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SellerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DealerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BusinessRegistrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LogoMediaReference = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    VerificationStatus = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceCenters_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServiceCenters_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    VehicleTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceTypes_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ChargingStationAmenities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChargingStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargingStationAmenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargingStationAmenities_ChargingStations_ChargingStationId",
                        column: x => x.ChargingStationId,
                        principalTable: "ChargingStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ChargingStationConnectors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChargingStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConnectorType = table.Column<int>(type: "int", nullable: false),
                    PowerKw = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Voltage = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    Amperage = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: true),
                    ChargingMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargingStationConnectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargingStationConnectors_ChargingStations_ChargingStationId",
                        column: x => x.ChargingStationId,
                        principalTable: "ChargingStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ChargingStationPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChargingStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConnectorType = table.Column<int>(type: "int", nullable: true),
                    PricingType = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargingStationPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargingStationPrices_ChargingStations_ChargingStationId",
                        column: x => x.ChargingStationId,
                        principalTable: "ChargingStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FuelStationAmenities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuelStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelStationAmenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelStationAmenities_FuelStations_FuelStationId",
                        column: x => x.FuelStationId,
                        principalTable: "FuelStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FuelStationFuelTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuelStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuelType = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelStationFuelTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelStationFuelTypes_FuelStations_FuelStationId",
                        column: x => x.FuelStationId,
                        principalTable: "FuelStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FuelStationWorkingHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuelStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    OpeningTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClosingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelStationWorkingHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelStationWorkingHours_FuelStations_FuelStationId",
                        column: x => x.FuelStationId,
                        principalTable: "FuelStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCenterBranches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCenterBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceCenterBranches_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServiceCenterBranches_ServiceCenters_ServiceCenterId",
                        column: x => x.ServiceCenterId,
                        principalTable: "ServiceCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ChargingStationAvailability",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChargingStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConnectorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AvailableFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvailableUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargingStationAvailability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargingStationAvailability_ChargingStationConnectors_ConnectorId",
                        column: x => x.ConnectorId,
                        principalTable: "ChargingStationConnectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ChargingStationAvailability_ChargingStations_ChargingStationId",
                        column: x => x.ChargingStationId,
                        principalTable: "ChargingStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FuelStationAvailability",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuelStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuelStationFuelTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelStationAvailability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelStationAvailability_FuelStationFuelTypes_FuelStationFuelTypeId",
                        column: x => x.FuelStationFuelTypeId,
                        principalTable: "FuelStationFuelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FuelStationAvailability_FuelStations_FuelStationId",
                        column: x => x.FuelStationId,
                        principalTable: "FuelStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FuelStationPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuelStationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuelStationFuelTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelStationPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelStationPrices_FuelStationFuelTypes_FuelStationFuelTypeId",
                        column: x => x.FuelStationFuelTypeId,
                        principalTable: "FuelStationFuelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FuelStationPrices_FuelStations_FuelStationId",
                        column: x => x.FuelStationId,
                        principalTable: "FuelStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCenterServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceCenterBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AutoServiceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriceFrom = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PriceTo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    EstimatedDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCenterServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceCenterServices_ServiceCenterBranches_ServiceCenterBranchId",
                        column: x => x.ServiceCenterBranchId,
                        principalTable: "ServiceCenterBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ServiceCenterServices_ServiceTypes_AutoServiceTypeId",
                        column: x => x.AutoServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServicePackages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceCenterBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    EstimatedDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePackages_ServiceCenterBranches_ServiceCenterBranchId",
                        column: x => x.ServiceCenterBranchId,
                        principalTable: "ServiceCenterBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ServiceWorkingHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceCenterBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    OpeningTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClosingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceWorkingHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceWorkingHours_ServiceCenterBranches_ServiceCenterBranchId",
                        column: x => x.ServiceCenterBranchId,
                        principalTable: "ServiceCenterBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ServiceBookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceCenterBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AutoServiceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ServicePackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CustomerNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CenterNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    EstimatedPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FinalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    ConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceBookings_ServiceCenterBranches_ServiceCenterBranchId",
                        column: x => x.ServiceCenterBranchId,
                        principalTable: "ServiceCenterBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceBookings_ServicePackages_ServicePackageId",
                        column: x => x.ServicePackageId,
                        principalTable: "ServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServiceBookings_ServiceTypes_AutoServiceTypeId",
                        column: x => x.AutoServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServiceBookings_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ServicePackageItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicePackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AutoServiceTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackageItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePackageItems_ServicePackages_ServicePackageId",
                        column: x => x.ServicePackageId,
                        principalTable: "ServicePackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ServicePackageItems_ServiceTypes_AutoServiceTypeId",
                        column: x => x.AutoServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceCenterBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ServiceBookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceReviews_ServiceBookings_ServiceBookingId",
                        column: x => x.ServiceBookingId,
                        principalTable: "ServiceBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceReviews_ServiceCenterBranches_ServiceCenterBranchId",
                        column: x => x.ServiceCenterBranchId,
                        principalTable: "ServiceCenterBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServiceReviews_ServiceCenters_ServiceCenterId",
                        column: x => x.ServiceCenterId,
                        principalTable: "ServiceCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationAmenities_ChargingStationId",
                table: "ChargingStationAmenities",
                column: "ChargingStationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationAmenities_CreatedAt",
                table: "ChargingStationAmenities",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationAmenities_IsDeleted",
                table: "ChargingStationAmenities",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationAvailability_ChargingStationId",
                table: "ChargingStationAvailability",
                column: "ChargingStationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationAvailability_ConnectorId",
                table: "ChargingStationAvailability",
                column: "ConnectorId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationAvailability_CreatedAt",
                table: "ChargingStationAvailability",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationAvailability_IsDeleted",
                table: "ChargingStationAvailability",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationAvailability_Status",
                table: "ChargingStationAvailability",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationConnectors_ChargingStationId",
                table: "ChargingStationConnectors",
                column: "ChargingStationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationConnectors_ConnectorType",
                table: "ChargingStationConnectors",
                column: "ConnectorType");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationConnectors_CreatedAt",
                table: "ChargingStationConnectors",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationConnectors_IsDeleted",
                table: "ChargingStationConnectors",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationConnectors_Status",
                table: "ChargingStationConnectors",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationPrices_ChargingStationId",
                table: "ChargingStationPrices",
                column: "ChargingStationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationPrices_CreatedAt",
                table: "ChargingStationPrices",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationPrices_EffectiveFrom",
                table: "ChargingStationPrices",
                column: "EffectiveFrom");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationPrices_IsActive",
                table: "ChargingStationPrices",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStationPrices_IsDeleted",
                table: "ChargingStationPrices",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStations_CreatedAt",
                table: "ChargingStations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStations_IsDeleted",
                table: "ChargingStations",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStations_LocationId",
                table: "ChargingStations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStations_Status",
                table: "ChargingStations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ChargingStations_VerificationStatus",
                table: "ChargingStations",
                column: "VerificationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationAmenities_CreatedAt",
                table: "FuelStationAmenities",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationAmenities_FuelStationId",
                table: "FuelStationAmenities",
                column: "FuelStationId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationAmenities_IsDeleted",
                table: "FuelStationAmenities",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationAvailability_CreatedAt",
                table: "FuelStationAvailability",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationAvailability_FuelStationFuelTypeId",
                table: "FuelStationAvailability",
                column: "FuelStationFuelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationAvailability_FuelStationId",
                table: "FuelStationAvailability",
                column: "FuelStationId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationAvailability_IsDeleted",
                table: "FuelStationAvailability",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationAvailability_Status",
                table: "FuelStationAvailability",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationFuelTypes_CreatedAt",
                table: "FuelStationFuelTypes",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationFuelTypes_FuelStationId_FuelType",
                table: "FuelStationFuelTypes",
                columns: new[] { "FuelStationId", "FuelType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationFuelTypes_IsDeleted",
                table: "FuelStationFuelTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationPrices_CreatedAt",
                table: "FuelStationPrices",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationPrices_EffectiveFrom",
                table: "FuelStationPrices",
                column: "EffectiveFrom");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationPrices_FuelStationFuelTypeId",
                table: "FuelStationPrices",
                column: "FuelStationFuelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationPrices_FuelStationId",
                table: "FuelStationPrices",
                column: "FuelStationId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationPrices_IsCurrent",
                table: "FuelStationPrices",
                column: "IsCurrent");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationPrices_IsDeleted",
                table: "FuelStationPrices",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStations_CreatedAt",
                table: "FuelStations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStations_IsDeleted",
                table: "FuelStations",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStations_LocationId",
                table: "FuelStations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStations_Status",
                table: "FuelStations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStations_VerificationStatus",
                table: "FuelStations",
                column: "VerificationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationWorkingHours_CreatedAt",
                table: "FuelStationWorkingHours",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationWorkingHours_FuelStationId_DayOfWeek",
                table: "FuelStationWorkingHours",
                columns: new[] { "FuelStationId", "DayOfWeek" });

            migrationBuilder.CreateIndex(
                name: "IX_FuelStationWorkingHours_IsDeleted",
                table: "FuelStationWorkingHours",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBookings_AutoServiceTypeId",
                table: "ServiceBookings",
                column: "AutoServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBookings_BookingReference",
                table: "ServiceBookings",
                column: "BookingReference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBookings_CreatedAt",
                table: "ServiceBookings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBookings_IsDeleted",
                table: "ServiceBookings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBookings_RequestedDate",
                table: "ServiceBookings",
                column: "RequestedDate");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBookings_ServiceCenterBranchId",
                table: "ServiceBookings",
                column: "ServiceCenterBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBookings_ServicePackageId",
                table: "ServiceBookings",
                column: "ServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBookings_Status",
                table: "ServiceBookings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBookings_UserId",
                table: "ServiceBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBookings_VehicleId",
                table: "ServiceBookings",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenterBranches_CreatedAt",
                table: "ServiceCenterBranches",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenterBranches_IsDeleted",
                table: "ServiceCenterBranches",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenterBranches_LocationId",
                table: "ServiceCenterBranches",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenterBranches_ServiceCenterId",
                table: "ServiceCenterBranches",
                column: "ServiceCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenters_CreatedAt",
                table: "ServiceCenters",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenters_DealerId",
                table: "ServiceCenters",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenters_IsDeleted",
                table: "ServiceCenters",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenters_SellerId",
                table: "ServiceCenters",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenters_Status",
                table: "ServiceCenters",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenters_VerificationStatus",
                table: "ServiceCenters",
                column: "VerificationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenterServices_AutoServiceTypeId",
                table: "ServiceCenterServices",
                column: "AutoServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenterServices_CreatedAt",
                table: "ServiceCenterServices",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenterServices_IsDeleted",
                table: "ServiceCenterServices",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenterServices_ServiceCenterBranchId_AutoServiceTypeId",
                table: "ServiceCenterServices",
                columns: new[] { "ServiceCenterBranchId", "AutoServiceTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackageItems_AutoServiceTypeId",
                table: "ServicePackageItems",
                column: "AutoServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackageItems_CreatedAt",
                table: "ServicePackageItems",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackageItems_IsDeleted",
                table: "ServicePackageItems",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackageItems_ServicePackageId_AutoServiceTypeId",
                table: "ServicePackageItems",
                columns: new[] { "ServicePackageId", "AutoServiceTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackages_CreatedAt",
                table: "ServicePackages",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackages_IsActive",
                table: "ServicePackages",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackages_IsDeleted",
                table: "ServicePackages",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackages_ServiceCenterBranchId",
                table: "ServicePackages",
                column: "ServiceCenterBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceReviews_CreatedAt",
                table: "ServiceReviews",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceReviews_IsDeleted",
                table: "ServiceReviews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceReviews_ServiceBookingId",
                table: "ServiceReviews",
                column: "ServiceBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceReviews_ServiceCenterBranchId",
                table: "ServiceReviews",
                column: "ServiceCenterBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceReviews_ServiceCenterId",
                table: "ServiceReviews",
                column: "ServiceCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceReviews_Status",
                table: "ServiceReviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceReviews_UserId",
                table: "ServiceReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTypes_Code",
                table: "ServiceTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTypes_CreatedAt",
                table: "ServiceTypes",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTypes_IsActive",
                table: "ServiceTypes",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTypes_IsDeleted",
                table: "ServiceTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTypes_VehicleTypeId",
                table: "ServiceTypes",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceWorkingHours_CreatedAt",
                table: "ServiceWorkingHours",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceWorkingHours_IsDeleted",
                table: "ServiceWorkingHours",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceWorkingHours_ServiceCenterBranchId_DayOfWeek",
                table: "ServiceWorkingHours",
                columns: new[] { "ServiceCenterBranchId", "DayOfWeek" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChargingStationAmenities");

            migrationBuilder.DropTable(
                name: "ChargingStationAvailability");

            migrationBuilder.DropTable(
                name: "ChargingStationPrices");

            migrationBuilder.DropTable(
                name: "FuelStationAmenities");

            migrationBuilder.DropTable(
                name: "FuelStationAvailability");

            migrationBuilder.DropTable(
                name: "FuelStationPrices");

            migrationBuilder.DropTable(
                name: "FuelStationWorkingHours");

            migrationBuilder.DropTable(
                name: "ServiceCenterServices");

            migrationBuilder.DropTable(
                name: "ServicePackageItems");

            migrationBuilder.DropTable(
                name: "ServiceReviews");

            migrationBuilder.DropTable(
                name: "ServiceWorkingHours");

            migrationBuilder.DropTable(
                name: "ChargingStationConnectors");

            migrationBuilder.DropTable(
                name: "FuelStationFuelTypes");

            migrationBuilder.DropTable(
                name: "ServiceBookings");

            migrationBuilder.DropTable(
                name: "ChargingStations");

            migrationBuilder.DropTable(
                name: "FuelStations");

            migrationBuilder.DropTable(
                name: "ServicePackages");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "ServiceCenterBranches");

            migrationBuilder.DropTable(
                name: "ServiceCenters");
        }
    }
}
