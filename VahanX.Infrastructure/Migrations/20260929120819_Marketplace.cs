using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VahanX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Marketplace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompareLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_CompareLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Municipality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
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
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Wishlists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dealers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LegalName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VerificationStatus = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Dealers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dealers_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PreferredContactMethod = table.Column<int>(type: "int", nullable: false),
                    SellerType = table.Column<int>(type: "int", nullable: false),
                    VerificationStatus = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sellers_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DealerBranches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DealerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
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
                    table.PrimaryKey("PK_DealerBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerBranches_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealerBranches_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DealerStaff",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DealerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_DealerStaff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerStaff_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleListings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DealerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false),
                    MileageUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ManufactureYear = table.Column<int>(type: "int", nullable: false),
                    RegistrationYear = table.Column<int>(type: "int", nullable: true),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    ContactPreference = table.Column<int>(type: "int", nullable: false),
                    IsNegotiable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_VehicleListings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleListings_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleListings_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_VehicleListings_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleListings_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompareItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompareListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_CompareItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompareItems_CompareLists_CompareListId",
                        column: x => x.CompareListId,
                        principalTable: "CompareLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompareItems_VehicleListings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "VehicleListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingMedia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MediaUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    MediaType = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_ListingMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingMedia_VehicleListings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "VehicleListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WishlistItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WishlistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_WishlistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishlistItems_VehicleListings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "VehicleListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WishlistItems_Wishlists_WishlistId",
                        column: x => x.WishlistId,
                        principalTable: "Wishlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompareItems_CompareListId",
                table: "CompareItems",
                column: "CompareListId");

            migrationBuilder.CreateIndex(
                name: "IX_CompareItems_CompareListId_ListingId",
                table: "CompareItems",
                columns: new[] { "CompareListId", "ListingId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompareItems_CreatedAt",
                table: "CompareItems",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CompareItems_IsDeleted",
                table: "CompareItems",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CompareItems_ListingId",
                table: "CompareItems",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_CompareLists_CreatedAt",
                table: "CompareLists",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CompareLists_IsDeleted",
                table: "CompareLists",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CompareLists_UserId",
                table: "CompareLists",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DealerBranches_CreatedAt",
                table: "DealerBranches",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DealerBranches_DealerId",
                table: "DealerBranches",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerBranches_IsActive",
                table: "DealerBranches",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DealerBranches_IsDeleted",
                table: "DealerBranches",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DealerBranches_LocationId",
                table: "DealerBranches",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_BusinessName",
                table: "Dealers",
                column: "BusinessName");

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_CreatedAt",
                table: "Dealers",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_IsActive",
                table: "Dealers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_IsDeleted",
                table: "Dealers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_LocationId",
                table: "Dealers",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_Phone",
                table: "Dealers",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_DealerStaff_CreatedAt",
                table: "DealerStaff",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DealerStaff_DealerId",
                table: "DealerStaff",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_DealerStaff_IsActive",
                table: "DealerStaff",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DealerStaff_IsDeleted",
                table: "DealerStaff",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DealerStaff_UserId",
                table: "DealerStaff",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingMedia_CreatedAt",
                table: "ListingMedia",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ListingMedia_IsDeleted",
                table: "ListingMedia",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ListingMedia_ListingId",
                table: "ListingMedia",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingMedia_ListingId_IsPrimary",
                table: "ListingMedia",
                columns: new[] { "ListingId", "IsPrimary" },
                unique: true,
                filter: "[IsPrimary] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CreatedAt",
                table: "Locations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_District",
                table: "Locations",
                column: "District");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_IsDeleted",
                table: "Locations",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Province",
                table: "Locations",
                column: "Province");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_CreatedAt",
                table: "Sellers",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_IsActive",
                table: "Sellers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_IsDeleted",
                table: "Sellers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_LocationId",
                table: "Sellers",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_Phone",
                table: "Sellers",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_UserId",
                table: "Sellers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleListings_CreatedAt",
                table: "VehicleListings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleListings_DealerId",
                table: "VehicleListings",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleListings_IsDeleted",
                table: "VehicleListings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleListings_LocationId",
                table: "VehicleListings",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleListings_ManufactureYear",
                table: "VehicleListings",
                column: "ManufactureYear");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleListings_Mileage",
                table: "VehicleListings",
                column: "Mileage");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleListings_Price",
                table: "VehicleListings",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleListings_SellerId",
                table: "VehicleListings",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleListings_Status",
                table: "VehicleListings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleListings_Status_CreatedAt",
                table: "VehicleListings",
                columns: new[] { "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleListings_Status_LocationId",
                table: "VehicleListings",
                columns: new[] { "Status", "LocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleListings_Status_Price",
                table: "VehicleListings",
                columns: new[] { "Status", "Price" });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleListings_VehicleId",
                table: "VehicleListings",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_CreatedAt",
                table: "WishlistItems",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_IsDeleted",
                table: "WishlistItems",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_ListingId",
                table: "WishlistItems",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_WishlistId",
                table: "WishlistItems",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_WishlistId_ListingId",
                table: "WishlistItems",
                columns: new[] { "WishlistId", "ListingId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_CreatedAt",
                table: "Wishlists",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_IsDeleted",
                table: "Wishlists",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompareItems");

            migrationBuilder.DropTable(
                name: "DealerBranches");

            migrationBuilder.DropTable(
                name: "DealerStaff");

            migrationBuilder.DropTable(
                name: "ListingMedia");

            migrationBuilder.DropTable(
                name: "WishlistItems");

            migrationBuilder.DropTable(
                name: "CompareLists");

            migrationBuilder.DropTable(
                name: "VehicleListings");

            migrationBuilder.DropTable(
                name: "Wishlists");

            migrationBuilder.DropTable(
                name: "Dealers");

            migrationBuilder.DropTable(
                name: "Sellers");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
