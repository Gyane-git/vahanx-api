using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VahanX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Trust : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccidentHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccidentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RepairStatus = table.Column<int>(type: "int", nullable: false),
                    RepairCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Source = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_AccidentHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccidentHistory_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DealerReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DealerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_DealerReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerReviews_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InspectionType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InspectorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OverallScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
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
                    table.PrimaryKey("PK_Inspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspections_VehicleListings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "VehicleListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Inspections_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PolicyReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_InsuranceHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsuranceHistory_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MileageHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false),
                    MileageUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    RecordedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_MileageHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MileageHistory_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OwnershipHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerSequence = table.Column<int>(type: "int", nullable: false),
                    OwnershipType = table.Column<int>(type: "int", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Source = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_OwnershipHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OwnershipHistory_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PriceHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NewPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_PriceHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceHistory_VehicleListings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "VehicleListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationStatus = table.Column<int>(type: "int", nullable: false),
                    RegistrationArea = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RegistrationReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Source = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_RegistrationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationHistory_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReviewReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
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
                    table.PrimaryKey("PK_ReviewReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SellerReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_SellerReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerReviews_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: true),
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    ServiceProvider = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Source = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_ServiceHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceHistory_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehicleReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_VehicleReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleReviews_VehicleListings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "VehicleListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_VehicleReviews_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehicleVerifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VerificationType = table.Column<int>(type: "int", nullable: false),
                    VerifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerificationReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
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
                    table.PrimaryKey("PK_VehicleVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleVerifications_VehicleListings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "VehicleListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_VehicleVerifications_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InspectionItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_InspectionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionItems_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Inspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InspectionReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverallScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Recommendations = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ReportStatus = table.Column<int>(type: "int", nullable: false),
                    ReportReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_InspectionReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionReports_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Inspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleVerificationStatusHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VerificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldStatus = table.Column<int>(type: "int", nullable: false),
                    NewStatus = table.Column<int>(type: "int", nullable: false),
                    ChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_VehicleVerificationStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleVerificationStatusHistory_VehicleVerifications_VerificationId",
                        column: x => x.VerificationId,
                        principalTable: "VehicleVerifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VerificationDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VerificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DocumentMediaReference = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_VerificationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerificationDocuments_VehicleVerifications_VerificationId",
                        column: x => x.VerificationId,
                        principalTable: "VehicleVerifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InspectionMedia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectionItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MediaReference = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    MediaType = table.Column<int>(type: "int", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_InspectionMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionMedia_InspectionItems_InspectionItemId",
                        column: x => x.InspectionItemId,
                        principalTable: "InspectionItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InspectionMedia_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Inspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccidentHistory_CreatedAt",
                table: "AccidentHistory",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AccidentHistory_IsDeleted",
                table: "AccidentHistory",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AccidentHistory_VehicleId",
                table: "AccidentHistory",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerReviews_CreatedAt",
                table: "DealerReviews",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DealerReviews_DealerId",
                table: "DealerReviews",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerReviews_IsDeleted",
                table: "DealerReviews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DealerReviews_Status",
                table: "DealerReviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DealerReviews_UserId",
                table: "DealerReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_CreatedAt",
                table: "InspectionItems",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_InspectionId",
                table: "InspectionItems",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItems_IsDeleted",
                table: "InspectionItems",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionMedia_CreatedAt",
                table: "InspectionMedia",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionMedia_InspectionId",
                table: "InspectionMedia",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionMedia_InspectionItemId",
                table: "InspectionMedia",
                column: "InspectionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionMedia_IsDeleted",
                table: "InspectionMedia",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionReports_CreatedAt",
                table: "InspectionReports",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionReports_InspectionId",
                table: "InspectionReports",
                column: "InspectionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionReports_IsDeleted",
                table: "InspectionReports",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_CreatedAt",
                table: "Inspections",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_InspectionType",
                table: "Inspections",
                column: "InspectionType");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_IsDeleted",
                table: "Inspections",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_ListingId",
                table: "Inspections",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_Status",
                table: "Inspections",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_VehicleId",
                table: "Inspections",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceHistory_CreatedAt",
                table: "InsuranceHistory",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceHistory_IsDeleted",
                table: "InsuranceHistory",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceHistory_VehicleId",
                table: "InsuranceHistory",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_MileageHistory_CreatedAt",
                table: "MileageHistory",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MileageHistory_IsDeleted",
                table: "MileageHistory",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MileageHistory_RecordedAt",
                table: "MileageHistory",
                column: "RecordedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MileageHistory_VehicleId",
                table: "MileageHistory",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipHistory_CreatedAt",
                table: "OwnershipHistory",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipHistory_FromDate",
                table: "OwnershipHistory",
                column: "FromDate");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipHistory_IsDeleted",
                table: "OwnershipHistory",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_OwnershipHistory_VehicleId",
                table: "OwnershipHistory",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_ChangedAt",
                table: "PriceHistory",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_CreatedAt",
                table: "PriceHistory",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_IsDeleted",
                table: "PriceHistory",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_ListingId",
                table: "PriceHistory",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationHistory_CreatedAt",
                table: "RegistrationHistory",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationHistory_IsDeleted",
                table: "RegistrationHistory",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationHistory_VehicleId",
                table: "RegistrationHistory",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewReports_CreatedAt",
                table: "ReviewReports",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewReports_IsDeleted",
                table: "ReviewReports",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewReports_ReportedBy",
                table: "ReviewReports",
                column: "ReportedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewReports_ReviewId",
                table: "ReviewReports",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerReviews_CreatedAt",
                table: "SellerReviews",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SellerReviews_IsDeleted",
                table: "SellerReviews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SellerReviews_SellerId",
                table: "SellerReviews",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerReviews_Status",
                table: "SellerReviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SellerReviews_UserId",
                table: "SellerReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistory_CreatedAt",
                table: "ServiceHistory",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistory_IsDeleted",
                table: "ServiceHistory",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistory_ServiceDate",
                table: "ServiceHistory",
                column: "ServiceDate");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistory_VehicleId",
                table: "ServiceHistory",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleReviews_CreatedAt",
                table: "VehicleReviews",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleReviews_IsDeleted",
                table: "VehicleReviews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleReviews_ListingId",
                table: "VehicleReviews",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleReviews_Status",
                table: "VehicleReviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleReviews_UserId",
                table: "VehicleReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleReviews_VehicleId",
                table: "VehicleReviews",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleVerifications_CreatedAt",
                table: "VehicleVerifications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleVerifications_IsDeleted",
                table: "VehicleVerifications",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleVerifications_ListingId",
                table: "VehicleVerifications",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleVerifications_Status",
                table: "VehicleVerifications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleVerifications_VehicleId",
                table: "VehicleVerifications",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleVerifications_VerificationType",
                table: "VehicleVerifications",
                column: "VerificationType");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleVerificationStatusHistory_ChangedAt",
                table: "VehicleVerificationStatusHistory",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleVerificationStatusHistory_CreatedAt",
                table: "VehicleVerificationStatusHistory",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleVerificationStatusHistory_IsDeleted",
                table: "VehicleVerificationStatusHistory",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleVerificationStatusHistory_VerificationId",
                table: "VehicleVerificationStatusHistory",
                column: "VerificationId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationDocuments_CreatedAt",
                table: "VerificationDocuments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationDocuments_IsDeleted",
                table: "VerificationDocuments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationDocuments_VerificationId",
                table: "VerificationDocuments",
                column: "VerificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccidentHistory");

            migrationBuilder.DropTable(
                name: "DealerReviews");

            migrationBuilder.DropTable(
                name: "InspectionMedia");

            migrationBuilder.DropTable(
                name: "InspectionReports");

            migrationBuilder.DropTable(
                name: "InsuranceHistory");

            migrationBuilder.DropTable(
                name: "MileageHistory");

            migrationBuilder.DropTable(
                name: "OwnershipHistory");

            migrationBuilder.DropTable(
                name: "PriceHistory");

            migrationBuilder.DropTable(
                name: "RegistrationHistory");

            migrationBuilder.DropTable(
                name: "ReviewReports");

            migrationBuilder.DropTable(
                name: "SellerReviews");

            migrationBuilder.DropTable(
                name: "ServiceHistory");

            migrationBuilder.DropTable(
                name: "VehicleReviews");

            migrationBuilder.DropTable(
                name: "VehicleVerificationStatusHistory");

            migrationBuilder.DropTable(
                name: "VerificationDocuments");

            migrationBuilder.DropTable(
                name: "InspectionItems");

            migrationBuilder.DropTable(
                name: "VehicleVerifications");

            migrationBuilder.DropTable(
                name: "Inspections");
        }
    }
}
