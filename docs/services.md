# VahanX Services Module

## Architecture

The Services module follows the same Clean Architecture principles as the rest of the VahanX platform.

## Service Center

### Overview
A ServiceCenter represents a business/company. A ServiceCenterBranch represents a physical location.

### Service Center Status
- Active
- Inactive
- Suspended
- PendingApproval

### Service Types
Database-driven master data for auto service types (General Service, Oil Change, Engine Service, etc.)

### Service Packages
Predefined packages with multiple service types and fixed pricing.

### Working Hours
Day-of-week based working hours with closed day support.

## Service Booking

### Overview
Service bookings at service center branches.

### Status Lifecycle
```
Requested → Pending → Confirmed → InProgress → Completed
                        → Rescheduled
                        → Cancelled
                        → Rejected
                        → NoShow
```

### Booking Reference
Auto-generated unique reference: `SB-YYYYMMDD-XXXXXXXX`

### Concurrency Protection
- Server-side availability check
- Branch status validation
- Working hours validation
- Unique booking reference

## EV Charging

### Overview
EV charging stations with connectors, amenities, availability, and pricing.

### Charging Station Status
- Active
- Inactive
- Maintenance
- TemporarilyUnavailable
- Closed

### Connector Types
- Type2, CCS2, CHAdeMO, GB/T AC, GB/T DC, NACS, Other

### Pricing Types
- PerKwh, PerMinute, PerSession, FlatRate

### Availability Status
- Available, Busy, Unavailable, Maintenance

## Fuel Station

### Overview
Fuel stations with fuel types, amenities, availability, and pricing.

### Fuel Station Status
- Active, Inactive, TemporarilyUnavailable, Closed

### Fuel Types
Station-specific inventory: Petrol, Diesel, Kerosene, LPG, CNG, Other

### Availability Status
- Available, Unavailable, Limited, Maintenance

## Nearby Search

### Overview
Location-based discovery for service centers, charging stations, and fuel stations.

### Parameters
- latitude
- longitude
- radiusKm

### Algorithm
Haversine formula for distance calculation.

## API Endpoints

### Service Centers
- `GET /api/v1/service-centers` - List with filters
- `GET /api/v1/service-centers/{id}` - Details
- `GET /api/v1/service-centers/nearby` - Nearby search
- `POST /api/v1/service-centers` - Create
- `PUT /api/v1/service-centers/{id}` - Update
- `DELETE /api/v1/service-centers/{id}` - Delete
- `GET/POST /api/v1/service-centers/{id}/branches` - Branch management
- `GET /api/v1/service-centers/branches/{branchId}/services` - Branch services
- `GET /api/v1/service-centers/branches/{branchId}/packages` - Branch packages
- `GET /api/v1/service-centers/branches/{branchId}/working-hours` - Working hours

### Service Bookings
- `GET /api/v1/service-bookings` - List with filters
- `GET /api/v1/service-bookings/{id}` - Details
- `GET /api/v1/service-bookings/my` - My bookings
- `POST /api/v1/service-bookings` - Create
- `PUT /api/v1/service-bookings/{id}` - Update
- `DELETE /api/v1/service-bookings/{id}` - Delete
- `POST /api/v1/service-bookings/{id}/confirm` - Confirm
- `POST /api/v1/service-bookings/{id}/reschedule` - Reschedule
- `POST /api/v1/service-bookings/{id}/start` - Start
- `POST /api/v1/service-bookings/{id}/complete` - Complete
- `POST /api/v1/service-bookings/{id}/cancel` - Cancel
- `POST /api/v1/service-bookings/{id}/reject` - Reject
- `POST /api/v1/service-bookings/{id}/no-show` - No-show

### Charging Stations
- `GET /api/v1/charging-stations` - List with filters
- `GET /api/v1/charging-stations/{id}` - Details
- `GET /api/v1/charging-stations/nearby` - Nearby search
- `GET /api/v1/charging-stations/{id}/connectors` - Connectors
- `GET /api/v1/charging-stations/{id}/amenities` - Amenities
- `GET /api/v1/charging-stations/{id}/availability` - Availability
- `GET /api/v1/charging-stations/{id}/prices` - Prices

### Fuel Stations
- `GET /api/v1/fuel-stations` - List with filters
- `GET /api/v1/fuel-stations/{id}` - Details
- `GET /api/v1/fuel-stations/nearby` - Nearby search
- `GET /api/v1/fuel-stations/{id}/fuel-types` - Fuel types
- `GET /api/v1/fuel-stations/{id}/availability` - Availability
- `GET /api/v1/fuel-stations/{id}/prices` - Prices
- `GET /api/v1/fuel-stations/{id}/amenities` - Amenities
- `GET /api/v1/fuel-stations/{id}/working-hours` - Working hours

## Database Relationships

```
ServiceCenter (1) ← (many) ServiceCenterBranch
ServiceCenterBranch (1) ← (many) ServiceCenterService, ServicePackage, ServiceWorkingHour, ServiceBooking
ServicePackage (1) ← (many) ServicePackageItem
ServiceType (1) ← (many) ServiceCenterService, ServicePackageItem
ChargingStation (1) ← (many) ChargingStationConnector, ChargingStationAmenity, ChargingStationAvailability, ChargingStationPrice
FuelStation (1) ← (many) FuelStationFuelType, FuelStationAmenity, FuelStationAvailability, FuelStationPrice, FuelStationWorkingHour
```

## Future Extension Points

- Payment integration
- Insurance marketplace
- Subscription services
- Analytics and reporting
- Admin dashboard
