# Simple Workbench

Simple Workbench is a local-first workspace for project notes, inline secrets, and search experimentation.

This repository currently contains:

- A `.NET 9` backend API (`apps/simple-workbench-api`)
- An Aspire AppHost orchestrator (`SimpleWorkbench.AppHost`)
- A lightweight web frontend shell (`apps/simple-workbench-web`)
- Reusable frontend package(s), including `@simple-workbench/note-builder`
- Unit, integration, and E2E test suites

## Tech Stack

### Backend

- **Language/Runtime:** C# / .NET 9 (`net9.0`)
- **Web Framework:** ASP.NET Core Minimal APIs
- **ORM:** Entity Framework Core `9.0.0`
- **Database Providers:** SQL Server `9.0.0`, SQLite `9.0.0` (primarily for integration tests)
- **Tests:** xUnit `2.9.2`, Microsoft.NET.Test.Sdk `17.12.0`

### Orchestration

- **Aspire AppHost SDK:** `13.0.1`

### Frontend / Tooling

- **Frontend language:** TypeScript + React (package-level components and tests)
- **Test runner:** Vitest `^4.1.9`
- **Component testing:** Testing Library (`@testing-library/react`, `@testing-library/user-event`)
- **E2E:** Playwright `^1.61.1`
- **Package manager:** npm (workspace-style repo layout)

## Repository Structure

```text
SimpleWorkbench/
├─ SimpleWorkbench.AppHost/                 # Aspire orchestrator
├─ apps/
│  ├─ simple-workbench-api/                 # .NET Web API
│  │  ├─ Api/
│  │  ├─ Application/
│  │  ├─ Domain/
│  │  ├─ Infrastructure/
│  │  └─ tests/
│  │     ├─ Unit/
│  │     └─ Integration/
│  └─ simple-workbench-web/                 # Web app shell
├─ packages/
│  └─ note-builder/                         # Reusable note builder UI package
└─ tests/
   └─ e2e/                                  # Playwright smoke/E2E tests
```

## Prerequisites

- **.NET SDK 9+**
- **Node.js 18+** (recommended 20+)
- **npm**
- **SQL Server LocalDB** (for SQL Server-backed API flows and EF tooling)

> Note: Integration tests use SQLite test databases by default through the test host configuration.

## Getting Started

From the repository root:

```bash
npm install
dotnet restore
```

## How to Run

### Option A: Run with Aspire AppHost (recommended)

```bash
dotnet run --project "SimpleWorkbench.AppHost/SimpleWorkbench.AppHost.csproj"
```

This starts:

- API project (`apps/simple-workbench-api`)
- Web app npm process (`apps/simple-workbench-web`, `npm run dev`)

### Option B: Run API only

```bash
dotnet run --project "apps/simple-workbench-api/SimpleWorkbench.Api.csproj"
```

Default health endpoint:

- `GET /health`

## Database & Migrations

Create a migration:

```bash
dotnet ef migrations add <MigrationName> \
  --project "apps/simple-workbench-api/SimpleWorkbench.Api.csproj" \
  --startup-project "apps/simple-workbench-api/SimpleWorkbench.Api.csproj" \
  --output-dir "Infrastructure/Migrations"
```

Design-time SQL Server connection is configured in:

- `apps/simple-workbench-api/Infrastructure/Persistence/SimpleWorkbenchDesignTimeDbContextFactory.cs`

## Testing

### Backend Unit Tests

```bash
dotnet test "apps/simple-workbench-api/tests/Unit/SimpleWorkbench.UnitTests.csproj"
```

### Backend Integration Tests

```bash
dotnet test "apps/simple-workbench-api/tests/Integration/SimpleWorkbench.IntegrationTests.csproj"
```

### Frontend Workspace Tests

```bash
npm run test --workspaces
```

### E2E Tests

```bash
npx playwright test "tests/e2e"
```

## Current Functional Scope

Implemented baseline capabilities include:

- Note and space persistence with EF Core migrations
- Note document validation and search-text extraction (with secret value stripping)
- Note CRUD/concurrency baseline
- Inline secret masking behavior (API + UI item)
- Home endpoint and home page sections (spaces, saved, recent, global notes)
- Lexical search endpoint baseline
- Hybrid score merge utility and matched-item jump/highlight utility
- Aspire orchestration + smoke E2E gate

## Notes

- This is an actively evolving project with incremental task-based development.
- Some modules are intentionally minimal scaffolds to support iterative delivery and testing.
