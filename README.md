# Newton Catalog

A two-page video game catalogue: a browse page and an edit page.

- Backend: ASP.NET Core 10, EF Core 10 (Code First, SQL Server)
- Frontend: Angular 22, Angular Router, ng-bootstrap / Bootstrap
- Tests: xUnit

## How it fits together

The OpenAPI document `contracts/newton-catalog.openapi.yaml` is the single source of truth.

```
contracts/newton-catalog.openapi.yaml
        |                         |
        | NSwag (dotnet build)    | openapi-typescript (npm run generate:api)
        v                         v
NewtonCatalog.Api/Generated/      NewtonCatalog.Web/src/app/games/api.ts
  GamesControllerBase (abstract)    Game, UpdateGameRequest, Platform, Genre types
  Game, UpdateGameRequest DTOs
```

Both generated files are created at build time and are not committed.

Backend layering: `GamesController` (overrides the generated base) -> `GameService` -> `GameRepository` -> `CatalogDbContext`.
All data access is EF Core LINQ; there is no hand-written SQL.
The generated `Game` DTO is also the EF entity, so the contract's `maxLength` values become column sizes.

Database: `SM1_InitialCreate` (schema migration) and `DM1_SeedInitialCatalog` (data migration, 100 games) run automatically on start-up.

## Prerequisites

- .NET SDK 10.0
- Node.js 24 (npm 11)
- SQL Server: LocalDB (`(localdb)\MSSQLLocalDB`, installed with Visual Studio) or SQL Server Express.
  For Express, change `ConnectionStrings:Default` in `NewtonCatalog.Api/appsettings.Development.json`.

## Run

API (creates the database, applies the migration, seeds 100 games):

```
dotnet run --project NewtonCatalog.Api
```

- API: http://localhost:5011/api/games
- Swagger UI: http://localhost:5011/swagger

Web app, in a second terminal:

```
cd NewtonCatalog.Web
npm install
npm start
```

Open http://localhost:4200. The dev server proxies `/api` to the API (`proxy.conf.json`), so no CORS setup is needed.

## Tests

```
dotnet test
```

Service tests run against EF Core's InMemory provider, controller tests use NSubstitute, and the contract validation tests check that the yaml constraints became `[Required]`, `[StringLength]` and `[Range]` attributes on the generated DTO.

## Changing the API

1. Edit `contracts/newton-catalog.openapi.yaml`.
2. `dotnet build` regenerates the C# controller base and DTOs; fix the compiler errors in `GamesController`.
3. `npm start` or `npm run build` regenerates the TypeScript types (`npm run generate:api` runs first automatically).
4. If a DTO field changed shape, add a schema migration: `dotnet tool restore` once, then
   `dotnet ef migrations add SM2_Name --project NewtonCatalog.Api -o Data/Migrations` and drop the timestamp prefix from the two new file names.

`generate:api` runs openapi-typescript through `npx` rather than as a dev dependency because its declared peer range stops at TypeScript 5 while Angular 22 uses TypeScript 6.

## Project layout

```
contracts/                 OpenAPI contract
NewtonCatalog.Api/         ASP.NET Core API
  Controllers/             GamesController
  Services/                IGameService, GameService
  Repositories/            IGameRepository, GameRepository
  Data/                    CatalogDbContext, Migrations (SM*), DataMigrations (DM*, seed data)
  nswag.json               NSwag settings for the build-time generation
NewtonCatalog.Api.Tests/   xUnit tests
NewtonCatalog.Web/         Angular app: games/game-list, games/game-edit, game-service
```
