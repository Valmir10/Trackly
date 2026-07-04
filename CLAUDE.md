# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Trackly is a multi-tenant SaaS all-in-one team workspace (tenants, users, projects, tasks, kanban boards, meeting orchestration, client communication, contract management) — the pitch is replacing the Slack/Azure-or-Jira/Teams/CRM fragmentation with one connected tool. It's a portfolio/learning project — the user is intentionally using it to learn the stack, so expect mid-task questions aimed at understanding, not just getting code written (answer them without losing the thread of the task). See `PRODUCT.md` and `DESIGN.md` at the repo root for the full product vision and visual design system — read them before doing design/UI work.

Monorepo with two independent halves that build, lint, and deploy separately:
- `backend/` — .NET 10 (C#) Clean Architecture API
- `frontend/` — React 19 + TypeScript + Vite SPA

## Commands

### Backend (run from `backend/`)
```
dotnet restore
dotnet build --configuration Release
dotnet test                                   # all test projects
dotnet test tests/Trackly.Domain.UnitTests    # single test project
dotnet test --filter "FullyQualifiedName~TenantTests"   # single test class/method
```
Requires the SDK version pinned in `backend/global.json` (currently 10.0.103). Solution file is `Trackly.slnx`.

### Frontend (run from `frontend/`)
```
npm install
npm run dev        # Vite dev server
npm run build      # tsc -b && vite build
npm run lint       # eslint .
npm run preview
```
No test script is wired up yet (`vitest` + `@testing-library/react` are installed as devDependencies but there's no `test` script or test files yet; CI's `npm run test --if-present` currently no-ops).

### Local infrastructure
`docker-compose up` at the repo root starts Postgres (5432), Redis (6379), MailHog (SMTP :1025, UI :8025), and MinIO (:9000, console :9001). Backend `appsettings` are expected to point at these for local dev.

### CI
GitHub Actions (`.github/workflows/`) run backend and frontend pipelines independently, each path-filtered so only the relevant workflow fires:
- `backend-ci.yml`: `dotnet restore` → `build --configuration Release` → `dotnet test`
- `frontend-ci.yml`: `npm ci` → `npm run build` → `npm run test --if-present`

## Backend architecture

Clean Architecture / Onion, four projects with a strict one-way dependency chain:

```
Trackly.Domain            (no project references — pure C#)
   ^
Trackly.Application        (references Domain)
   ^
Trackly.Infrastructure      (references Application)
   ^
Trackly.Api                (references Application + Infrastructure)
```

Never let Domain reference Application/Infrastructure, or Application reference Infrastructure — that inverts the dependency chain.

- **Domain** (`src/Trackly.Domain`): entities, enums, domain events, exceptions. No external packages. Entities inherit `Common.Entity` (Guid `Id`, identity-based equality) or `Common.AggregateRoot` (adds a domain-event buffer via `AddDomainEvent`/`ClearDomainEvents`). Entities use private constructors + static `Create(...)` factory methods that validate and raise a `*CreatedEvent`; all mutation happens through named methods (`UpdateName`, `SetRole`, etc.), never public setters. Domain exceptions derive from the abstract `DomainException`.
- **Application**: intended to hold MediatR commands/queries, FluentValidation validators, AutoMapper profiles (packages already referenced; not yet populated beyond a smoke test).
- **Infrastructure**: EF Core + Npgsql (Postgres), BCrypt.Net for password hashing, Serilog for logging (console + file sinks). Intended home for `DbContext`, repositories, and other outbound integrations.
- **Api** (`Trackly.Api`): ASP.NET Core minimal API host. JWT bearer auth and Scalar (OpenAPI UI) packages are referenced. `Program.cs` currently still has the default minimal-API template (`/weatherforecast`) — not yet wired to the real domain.

Testing mirrors the project layout 1:1 (`Trackly.Domain.UnitTests`, `Trackly.Application.UnitTests`, `Trackly.Api.IntegrationTests`), each referencing only its matching src project (except integration tests, which reference `Trackly.Api` and pull in `Testcontainers.PostgreSql` for real-DB tests). Stack: xunit + FluentAssertions + NSubstitute. Domain unit tests use `Should()`-style fluent assertions and group cases under `// ---` comment banners per method under test.

## Frontend architecture

- **Routing** (`src/App.tsx`): `react-router-dom`, all routes declared in one file. Tenant-scoped pages are nested under a `/:slug/...` prefix (`/:slug/dashboard`, `/:slug/projects`, `/:slug/projects/:projectId`, `/:slug/tasks`, `/:slug/analytics`, `/:slug/settings`); public/auth pages (`/`, `/login`, `/register`, `/forgot-password`, `/verify-email`, `/invite/:token`, `/onboarding`) are not slug-scoped.
- **Pages vs components**: `src/pages/` holds one component per route; `src/components/` holds everything reused/composed into pages, with a `components/ui/` subfolder for shadcn/radix-based primitives (button, card, badge, avatar, etc.).
- **Styling**: mid-migration off Tailwind CSS v4 onto hand-authored vanilla CSS (custom properties, no utility framework) — see `DESIGN.md` for the token system and rules. Pages/components not yet migrated still use Tailwind utility classes and shadcn/ui (`components.json`, style `new-york`, base color `neutral`) — treat those as legacy being replaced surface-by-surface, not the pattern to extend. Don't add new Tailwind classes or shadcn components to any surface being redesigned.
- **Path alias**: `@/*` → `src/*` (set in both `vite.config.ts` and `components.json`); always import via `@/...` rather than relative `../../`.
- **State/data libs available**: `zustand` (client state), `@tanstack/react-query` + `axios` (server state/data fetching), `react-hook-form` + `zod` + `@hookform/resolvers` (forms), `@microsoft/signalr` (realtime — presumably for live task/board updates), `@dnd-kit/*` (drag-and-drop — presumably for kanban), `recharts` (analytics charts), `framer-motion` (animation). None of these have an established usage pattern yet in the codebase — check for precedent in a similar component before introducing a new one.
- **Component style**: keep components thin (JSX + minimal props); push non-trivial logic into `src/utils/` if it's pure, or a custom hook in `src/hooks/` if it needs React state/effects (see `src/hooks/useTheme.ts` for the hook pattern — reads/writes `localStorage`, toggles the `dark` class on `documentElement`). `src/utils/` currently exists but is empty.
- Theme is dark-mode-by-default: `main.tsx` seeds the `dark` class on `<html>` before React even mounts, based on `localStorage.getItem('theme')`.

## Git workflow (from prior sessions with this user)

- One branch per page, per global/cross-cutting change, or per complex feature — don't mix unrelated pages/features on one branch.
- Commit style: many small, frequent commits with short plain messages; no AI attribution/signature lines in commit messages.
- Keep git commands clean — no `2>&1` or shell redirects tacked onto git commands (especially `git push`).
