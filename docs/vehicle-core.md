# Vehicle Core Module

## Overview

The Vehicle Core module provides the foundational vehicle master data for the VahanX platform. It defines the canonical vehicle hierarchy from VehicleType down to Vehicle, along with all supporting master data entities.

## Architecture

### Entity Relationships

```
VehicleType
    └── VehicleCategory

Brand
    └── Model
          └── Generation
                └── Variant
                      └── Vehicle

Vehicle
    ├── BodyType
    ├── FuelType
    ├── TransmissionType
    ├── DriveType
    └── EngineType

Variant/Vehicle
    ├── VehicleFeature (many-to-many)
    └── VehicleSpecification (many-to-many)
```

### Entity Descriptions

| Entity | Description | Table |
|--------|-------------|-------|
| VehicleType | Vehicle type master data (Car, Bike, Scooter, SUV, etc.) | VehicleTypes |
| VehicleCategory | Vehicle category belonging to a VehicleType | VehicleCategories |
| Brand | Vehicle brand (Toyota, Honda, Yamaha, etc.) | Brands |
| Model | Vehicle model belonging to a Brand | Models |
| Generation | Model generation (10th Gen, 11th Gen, etc.) | Generations |
| Variant | Vehicle variant belonging to a Generation | Variants |
| Vehicle | Canonical vehicle definition | Vehicles |
| BodyType | Body type master data (Sedan, Hatchback, SUV, etc.) | BodyTypes |
| FuelType | Fuel type master data (Petrol, Diesel, Electric, etc.) | FuelTypes |
| TransmissionType | Transmission type master data (Manual, Automatic, etc.) | TransmissionTypes |
| DriveType | Drive type master data (FWD, RWD, AWD, 4WD) | DriveTypes |
| EngineType | Engine type master data (ICE, Electric Motor, etc.) | EngineTypes |
| VehicleFeature | Reusable vehicle feature (ABS, Airbags, Sunroof, etc.) | VehicleFeatures |
| VehicleSpecification | Vehicle specification definition (Engine Capacity, Power, etc.) | VehicleSpecifications |

## API Endpoints

### Vehicle Types
- `GET /api/v1/vehicle-types` - Get all vehicle types (paginated)
- `GET /api/v1/vehicle-types/{id}` - Get vehicle type by ID
- `POST /api/v1/vehicle-types` - Create vehicle type
- `PUT /api/v1/vehicle-types/{id}` - Update vehicle type
- `DELETE /api/v1/vehicle-types/{id}` - Delete vehicle type

### Vehicle Categories
- `GET /api/v1/vehicle-categories` - Get all vehicle categories (paginated)
- `GET /api/v1/vehicle-categories/{id}` - Get vehicle category by ID
- `POST /api/v1/vehicle-categories` - Create vehicle category
- `PUT /api/v1/vehicle-categories/{id}` - Update vehicle category
- `DELETE /api/v1/vehicle-categories/{id}` - Delete vehicle category

### Brands
- `GET /api/v1/brands` - Get all brands (paginated)
- `GET /api/v1/brands/{id}` - Get brand by ID
- `POST /api/v1/brands` - Create brand
- `PUT /api/v1/brands/{id}` - Update brand
- `DELETE /api/v1/brands/{id}` - Delete brand

### Models
- `GET /api/v1/models` - Get all models (paginated)
- `GET /api/v1/models/{id}` - Get model by ID
- `POST /api/v1/models` - Create model
- `PUT /api/v1/models/{id}` - Update model
- `DELETE /api/v1/models/{id}` - Delete model

### Generations
- `GET /api/v1/generations` - Get all generations (paginated)
- `GET /api/v1/generations/{id}` - Get generation by ID
- `POST /api/v1/generations` - Create generation
- `PUT /api/v1/generations/{id}` - Update generation
- `DELETE /api/v1/generations/{id}` - Delete generation

### Variants
- `GET /api/v1/variants` - Get all variants (paginated)
- `GET /api/v1/variants/{id}` - Get variant by ID
- `POST /api/v1/variants` - Create variant
- `PUT /api/v1/variants/{id}` - Update variant
- `DELETE /api/v1/variants/{id}` - Delete variant

### Vehicles
- `GET /api/v1/vehicles` - Get all vehicles (paginated)
- `GET /api/v1/vehicles/{id}` - Get vehicle by ID
- `POST /api/v1/vehicles` - Create vehicle
- `PUT /api/v1/vehicles/{id}` - Update vehicle
- `DELETE /api/v1/vehicles/{id}` - Delete vehicle

### Master Data
- `GET /api/v1/master-data/body-types` - Get all body types
- `GET /api/v1/master-data/fuel-types` - Get all fuel types
- `GET /api/v1/master-data/transmissions` - Get all transmission types
- `GET /api/v1/master-data/drive-types` - Get all drive types
- `GET /api/v1/master-data/engine-types` - Get all engine types
- `GET /api/v1/master-data/features` - Get all vehicle features
- `GET /api/v1/master-data/specifications` - Get all vehicle specifications

## Database Migration

Migration name: `VehicleCore`

```bash
dotnet ef migrations add VehicleCore --project VahanX.Infrastructure --startup-project VahanX.Api
dotnet ef database update --project VahanX.Infrastructure --startup-project VahanX.Api
```

## Seed Data

The following seed data is available for development:

**Vehicle Types:** Car, Bike, Scooter, SUV, Bus, Truck, EV

**Body Types:** Sedan, Hatchback, SUV, Pickup, Van

**Fuel Types:** Petrol, Diesel, Electric, Hybrid

**Transmission Types:** Manual, Automatic, CVT

**Drive Types:** FWD, RWD, AWD, 4WD

**Engine Types:** ICE, Electric Motor, Hybrid, Plug-in Hybrid

## Testing

```bash
dotnet test
```

## Design Decisions

1. **Database-driven master data:** VehicleType, BodyType, FuelType, etc. are database-driven to allow admin management without code changes.

2. **Soft delete:** All entities inherit from BaseEntity which supports soft delete via IsDeleted flag.

3. **Restrict delete behavior:** Foreign keys use Restrict delete behavior to prevent accidental deletion of master data trees.

4. **Repository pattern:** Generic repository pattern is used to maintain Clean Architecture principles.

5. **Separate Vehicle from VehicleListing:** Vehicle is the canonical vehicle definition, not a marketplace listing. VehicleListing will be implemented in a future phase.
