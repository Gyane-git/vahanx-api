# VahanX Architecture

## Overview

VahanX follows Clean Architecture (Onion Architecture) principles with clear separation of concerns and dependency direction.

## Dependency Direction

```
┌─────────────────────────────────────────┐
│              VahanX.Api                 │
│  (Controllers, Middleware, Extensions)  │
└─────────────────┬───────────────────────┘
                  │ depends on
┌─────────────────▼───────────────────────┐
│         VahanX.Infrastructure           │
│  (EF Core, Repositories, Services)     │
└─────────────────┬───────────────────────┘
                  │ depends on
┌─────────────────▼───────────────────────┐
│         VahanX.Application              │
│  (DTOs, Interfaces, Validators, Svcs)   │
└─────────────────┬───────────────────────┘
                  │ depends on
┌─────────────────▼───────────────────────┐
│           VahanX.Domain                 │
│  (Entities, Enums, ValueObjects, Excs)  │
└─────────────────────────────────────────┘
```

## Layer Responsibilities

### Domain Layer (VahanX.Domain)

- **Entities**: Core business entities (BaseEntity, AuditLog)
- **Enums**: Domain enumerations (AuditAction)
- **Value Objects**: Immutable value objects
- **Exceptions**: Domain-specific exceptions
- **No dependencies** on any other layer

### Application Layer (VahanX.Application)

- **DTOs**: Data transfer objects for API communication
- **Interfaces**: Service interfaces (contracts)
- **Validators**: FluentValidation validators
- **Services**: Application service implementations
- **Common**: Shared models (ApiResponse, PagedResult)
- **Depends on**: Domain only

### Infrastructure Layer (VahanX.Infrastructure)

- **Persistence**: DbContext, entity configurations
- **Repositories**: Data access implementations
- **Services**: Infrastructure services (health checks)
- **Depends on**: Application + Domain

### API Layer (VahanX.Api)

- **Controllers**: Thin controllers (no business logic)
- **Middleware**: Global exception handling, request timing
- **Extensions**: Service registration extensions
- **Configuration**: Settings classes
- **Filters**: Action filters
- **Depends on**: All layers

## Key Design Decisions

### 1. Base Entity

All entities inherit from `BaseEntity` which provides:
- `Id` (Guid)
- `CreatedAt`, `UpdatedAt` (UTC DateTime)
- `CreatedBy`, `UpdatedBy` (string)
- `IsDeleted`, `DeletedAt`, `DeletedBy` (soft delete)

### 2. Soft Delete

- Implemented via `IsDeleted` flag
- Query filter automatically excludes deleted records
- Deleted records can be accessed by disabling the filter

### 3. Audit Logging

- `AuditLog` entity tracks all significant actions
- Designed for future authentication integration
- Captures: UserId, Action, EntityName, EntityId, OldValues, NewValues, IpAddress, UserAgent

### 4. API Response Standard

All endpoints return `ApiResponse<T>`:
```json
{
  "success": true,
  "message": "Request successful",
  "data": {},
  "errors": [],
  "traceId": "..."
}
```

### 5. Global Exception Handling

- `GlobalExceptionMiddleware` catches all unhandled exceptions
- Returns consistent error responses
- Never exposes stack traces in production
- Logs all exceptions with trace IDs

### 6. API Versioning

- URL-based versioning: `/api/v1/...`
- Header-based versioning: `X-API-Version`
- Easy to add v2 without breaking v1

### 7. Health Checks

- `/health` - Liveness probe
- `/health/ready` - Readiness probe (includes DB check)

## Future Phase 2+ Considerations

The foundation is designed to support:
- JWT Authentication & Authorization
- Role-based and permission-based access
- Vehicle listings and management
- Search and filtering
- Wishlist and compare
- Finance and services
- EV charging
- Reviews and ratings
- Messaging and notifications
- And more...

## Database Design Rules

- Primary keys: Guid
- Timestamps: UTC DateTime
- Soft delete: IsDeleted flag
- Audit fields: CreatedAt, UpdatedAt, CreatedBy, UpdatedBy
- Indexes: On frequently queried columns
- Foreign keys: Explicit configuration
- Decimal precision: For financial values
