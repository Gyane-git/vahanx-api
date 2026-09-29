# VahanX API

Nepal's Automotive Marketplace Platform - Production Foundation

## Project Overview

VahanX is a production-grade Nepal-focused automotive marketplace platform. This repository contains the Phase 1 production foundation built with .NET 10, ASP.NET Core Web API, Entity Framework Core, and SQL Server.

## Architecture

The solution follows Clean Architecture principles with clear dependency direction:

```
VahanX.Domain (innermost - no dependencies)
    ↓
VahanX.Application (depends on Domain)
    ↓
VahanX.Infrastructure (depends on Application + Domain)
    ↓
VahanX.Api (depends on all)
```

### Project Structure

```
VahanX/
├── VahanX.Api/              # Web API layer (controllers, middleware, extensions)
├── VahanX.Application/      # Application layer (DTOs, interfaces, validators, services)
├── VahanX.Domain/           # Domain layer (entities, enums, value objects, exceptions)
├── VahanX.Infrastructure/   # Infrastructure layer (EF Core, repositories, services)
├── VahanX.Tests/            # Unit and integration tests
├── docker-compose.yml       # Docker orchestration
└── README.md
```

## Requirements

- .NET 10 SDK
- Docker Desktop
- SQL Server 2022 (via Docker)

## Local Setup

### 1. Clone and Restore

```bash
git clone <repository-url>
cd vahanx-api
dotnet restore
```

### 2. Configure Connection String

Update `VahanX.Api/appsettings.Development.json` with your SQL Server credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1445;Database=VahanXDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=True;"
  }
}
```

### 3. Run SQL Server via Docker

```bash
docker-compose up -d vahanx-sqlserver
```

### 4. Apply Migrations

```bash
dotnet ef database update --project VahanX.Infrastructure --startup-project VahanX.Api
```

### 5. Run the API

```bash
dotnet run --project VahanX.Api
```

## Docker Setup

### Start All Services

```bash
docker-compose up -d
```

### View Logs

```bash
docker-compose logs -f vahanx-api
```

### Stop Services

```bash
docker-compose down
```

### Stop and Remove Volumes

```bash
docker-compose down -v
```

## SQL Server Configuration

| Setting | Value |
|---------|-------|
| Host Port | 1445 |
| Container Port | 1433 |
| Database | VahanXDb |
| Username | sa |
| Password | YourStrong!Passw0rd (change in production) |
| Volume | vahanx_sql_data |

## Environment Variables

| Variable | Description | Required |
|----------|-------------|----------|
| `ConnectionStrings__DefaultConnection` | SQL Server connection string | Yes |
| `ASPNETCORE_ENVIRONMENT` | Environment name (Development/Staging/Production) | No |
| `ASPNETCORE_URLS` | API URLs | No |

## EF Migrations

### Create Migration

```bash
dotnet ef migrations add <MigrationName> --project VahanX.Infrastructure --startup-project VahanX.Api
```

### Apply Migration

```bash
dotnet ef database update --project VahanX.Infrastructure --startup-project VahanX.Api
```

### Remove Last Migration

```bash
dotnet ef migrations remove --project VahanX.Infrastructure --startup-project VahanX.Api
```

## Running the API

### Development

```bash
dotnet run --project VahanX.Api
```

### Docker

```bash
docker-compose up -d
```

## Running Tests

```bash
dotnet test
```

## Swagger

Access Swagger UI at:

- Local: `http://localhost:5001/swagger`
- Docker: `http://localhost:5001/swagger`

## Health Endpoints

| Endpoint | Description |
|----------|-------------|
| `GET /health` | Liveness probe |
| `GET /health/ready` | Readiness probe (includes DB check) |

## API Endpoints

| Endpoint | Description |
|----------|-------------|
| `GET /api/v1/system/info` | System information |
| `GET /api/v1/health` | Health check |
| `GET /api/v1/health/ready` | Readiness check |
| `GET /api/v1/vehicle-types` | Vehicle types |
| `GET /api/v1/vehicle-categories` | Vehicle categories |
| `GET /api/v1/brands` | Brands |
| `GET /api/v1/models` | Models |
| `GET /api/v1/generations` | Generations |
| `GET /api/v1/variants` | Variants |
| `GET /api/v1/vehicles` | Vehicles |
| `GET /api/v1/master-data/body-types` | Body types |
| `GET /api/v1/master-data/fuel-types` | Fuel types |
| `GET /api/v1/master-data/transmissions` | Transmission types |
| `GET /api/v1/master-data/drive-types` | Drive types |
| `GET /api/v1/master-data/engine-types` | Engine types |
| `GET /api/v1/master-data/features` | Vehicle features |
| `GET /api/v1/master-data/specifications` | Vehicle specifications |

## Development Workflow

1. Create feature branch
2. Make changes following Clean Architecture principles
3. Add/update tests
4. Run `dotnet format`
5. Run `dotnet build`
6. Run `dotnet test`
7. Commit and push

## License

Proprietary - VahanX
