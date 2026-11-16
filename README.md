# NEXUS CMS — API

.NET 10 backend for the NEXUS CMS headless content management system. Clean Architecture, CQRS, PostgreSQL.

## Tech Stack

- **.NET 10** — ASP.NET Core Web API
- **Clean Architecture** — Domain · Application · Persistence · API
- **CQRS** + **MediatR**
- **Entity Framework Core** + **Npgsql** (PostgreSQL, JSONB)
- **ASP.NET Core Identity** + **JWT** (access + refresh token)
- **FluentValidation**
- **Serilog**
- **Result Pattern** (exception-free flow)

## Getting Started

### Prerequisites

- .NET 10 SDK
- PostgreSQL 15+

### Configuration

Copy the example config and fill in your values:

```bash
cp src/NexusCms.Api/appsettings.example.json src/NexusCms.Api/appsettings.json
```

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=nexuscms;Username=postgres;Password=your_password"
  },
  "Jwt": {
    "Key": "at-least-32-character-random-secret-key",
    "Issuer": "NexusCms",
    "Audience": "NexusCms",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "Encryption": {
    "Key": "32-character-aes-key-here!!"
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:3000"]
  }
}
```

### Run

```bash
cd src/NexusCms.Api

# Restore & build
dotnet restore && dotnet build

# Apply migrations
dotnet ef database update --project ../NexusCms.Persistence --startup-project .

# Start (migrations are also applied automatically on startup)
dotnet run
# http://localhost:5281
```

### Migrations

```bash
# Add new migration
dotnet ef migrations add <MigrationName> \
  --project src/NexusCms.Persistence \
  --startup-project src/NexusCms.Api

# Apply
dotnet ef database update \
  --project src/NexusCms.Persistence \
  --startup-project src/NexusCms.Api
```

## Docker

From the repository root (runs api + panel + postgres):

```bash
docker compose up
```

Set real values in `docker-compose.yml` before deploying:

```yaml
Jwt__Key: "your-strong-secret-at-least-32-chars"
Encryption__Key: "your-32-char-key"
```

## Project Structure

```
src/
├── NexusCms.Domain/
│   ├── Common/          # Entity base, Result<T>, value objects
│   ├── Entities/        # PageModel, PageInstance, FileAsset, User, Role, Menu…
│   ├── Enums/
│   └── Interfaces/      # Repository interfaces, IUnitOfWork
│
├── NexusCms.Application/
│   ├── Common/
│   │   ├── Behaviors/   # Logging, validation, auth pipeline behaviors
│   │   └── Interfaces/  # IJwtTokenGenerator, IFileStorageService…
│   ├── PageModels/      # Schema CRUD
│   ├── PageInstances/   # Content CRUD + publish/archive
│   ├── Files/           # Media upload/list/delete
│   ├── Menus/
│   ├── Settings/        # AppSetting singleton
│   ├── Roles/           # Dynamic role & permission management
│   ├── Users/
│   ├── Public/          # Public read-only queries
│   └── Dashboard/
│
├── NexusCms.Persistence/
│   ├── Configurations/  # EF Core entity configs, JSONB mappings
│   ├── Repositories/
│   ├── Migrations/
│   └── AppDbContext.cs
│
└── NexusCms.Api/
    ├── Controllers/
    │   ├── Admin/       # Protected endpoints (JWT + permission)
    │   └── Public/      # Public read-only endpoints
    ├── Authorization/   # [RequirePermission] attribute & handler
    ├── Middleware/      # Exception, ViewCount tracking
    ├── Services/        # JwtTokenGenerator, FileStorageService…
    └── Program.cs
```

## API Overview

### Public (no auth)

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/public/pages/{modelKey}` | List published pages |
| GET | `/api/public/pages/{modelKey}/{slug}` | Single page + view count |
| GET | `/api/public/menus/{key}` | Menu tree |
| GET | `/api/public/settings` | Site settings |
| GET | `/api/public/languages` | Active languages |

### Admin (JWT required + permission)

| Area | Base path |
|---|---|
| Auth | `/api/admin/auth` |
| Page Models | `/api/admin/page-models` |
| Content | `/api/admin/pages` |
| Media | `/api/admin/files` |
| Menus | `/api/admin/menus` |
| Settings | `/api/admin/settings` |
| Languages | `/api/admin/languages` |
| Users | `/api/admin/users` |
| Roles | `/api/admin/roles` |
| Activity | `/api/admin/activity` |
| Mail | `/api/admin/mail` |
| Contact | `/api/admin/contact` |

All list endpoints accept `?search=&page=&pageSize=&sortBy=&sortDir=` and have a corresponding `/export?format=csv|xlsx` endpoint.

## Authorization

Every admin endpoint is decorated with `[RequirePermission("module.action")]`. Permissions are seeded on startup and assigned to roles via the panel. The `SuperAdmin` role receives all permissions automatically and cannot be modified.

## Content Schema

Page content is stored as JSONB, keyed by language:

```json
{
  "en": { "hero": { "title": "Heading", "image": "/uploads/images/..." } },
  "de": { "hero": { "title": "Überschrift", "image": "/uploads/images/..." } }
}
```

The public API returns the full `contentJson` — the frontend selects the desired language key.

## Default Login

After the first start the API seeds a SuperAdmin user: `admin@nexuscms.local` / `Admin1234!`. **Change this password immediately.**

## Contributing

NEXUS CMS is open source and contributions are welcome. Fork the repo, create a feature branch, make sure the build passes, and open a Pull Request describing what changed and why. Bug reports and feature ideas are welcome as issues.

Related repository — Admin panel: [cms-panel](https://github.com/hasamuddinafz/cms-panel)

## License

[MIT](LICENSE) © Hasamuddin Afzali
