# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

`CompanyProfileMVC` is an ASP.NET Core MVC application (.NET 10, C#) for a company profile / blog site. It has a public-facing site (home page, login page) and an `Admin` area for content management, backed by PostgreSQL via EF Core (Npgsql).

## Commands

```bash
dotnet restore                 # restore NuGet packages
dotnet build                   # build the project
dotnet run                     # run (see Properties/launchSettings.json for ports/profiles)
dotnet watch run                # run with hot reload
```

Launch profiles (`Properties/launchSettings.json`): `http` on `http://localhost:5145`, `https` on `https://localhost:7251` / `http://localhost:5145`. Both set `ASPNETCORE_ENVIRONMENT=Development`.

EF Core migrations (requires the `dotnet-ef` tool: `dotnet tool install --global dotnet-ef` if not already installed):

```bash
dotnet ef migrations add <Name>
dotnet ef database update
```

There is no test project in this repository yet. There is no configured lint/format script for C#; `.editorconfig` (2-space indent, LF, UTF-8) applies repo-wide, and `.prettierrc.json`/`.prettierignore` are present for formatting JS/CSS/JSON assets but aren't wired to an npm script (no `package.json`).

## Configuration

- The PostgreSQL connection string (`DefaultConnection`) lives in `appsettings.Development.json`, pointing at a local `net-mvc-blog` database. It is **not** present in `appsettings.json`, so it must be supplied (via environment/user-secrets/other appsettings) for any non-Development run.

## Architecture

### Routing (`Program.cs`)

Two route maps are registered, in this order, so area routes take precedence:

1. `"areas"` — `{area:exists}/{controller=Home}/{action=Index}/{id?}`
2. `"default"` — `{controller=Home}/{action=Index}/{id?}`

Static assets use the newer `MapStaticAssets()` / `.WithStaticAssets()` pipeline rather than `UseStaticFiles()`.

### Public site vs. Admin area

- **Public site**: `Controllers/` (`HomeController`, `LoginController`), `Views/Home`, `Views/Login`, shared layout at `Views/Shared/_Layout.cshtml`. `LoginController.Index` currently only renders the view — the POST handler is commented out (no auth is wired up yet).
- **Admin area** (`Areas/Admin/`): reached via the `{area:exists}` route; every controller is decorated with `[Area("Admin")]`. It uses its own layout (`Areas/Admin/Views/Shared/_Layout.cshtml`, `_ContentNavbarLayout.cshtml`) built on the Sneat/Materio Bootstrap admin template, with reusable chrome in `Views/Shared/Sections/{Navbar,Menu,Footer}` and template helpers in `Views/_Partials/_Macros.cshtml`. Admin views reference template assets under `~/vendor/...`, but `wwwroot/vendor/**/*.css` and `**/*.js` are gitignored — those theme assets are not committed and must be sourced separately for the admin UI to render fully.

**Namespace quirk to preserve**: controller classes under `Areas/Admin/Controllers/*.cs` declare namespace `CompanyProfileMVC.Controllers.Admin` (not `...Areas.Admin.Controllers`), while models under `Areas/Admin/Models/` declare `CompanyProfileMVC.Areas.Admin.Models`. This is inconsistent but established — match the existing namespace per folder when adding files rather than "fixing" it as a drive-by.

### Data layer

- `Data/AppDbContext.cs` — single `DbContext`; entities are added as `DbSet<T>` properties.
- `Data/Entities/` — plain entity classes (e.g. `Category`).
- `Migrations/` — EF Core migrations (one so far, `AddCategory`).

### Admin CRUD convention

The `Category` feature (`Areas/Admin/Controllers/CategoryController.cs`, `Areas/Admin/Models/Category*ViewModel.cs`, `Areas/Admin/Views/Category/Index.cshtml`) is the template to copy for new admin-managed entities:

- Controller: `[Area("Admin")]`, primary-constructor DI for `AppDbContext`, a `private const int PageSize` for list pagination.
- Four view models per entity: `<Entity>ViewModel` (read DTO), `Create<Entity>ViewModel` / `Edit<Entity>ViewModel` (with `[Required]` data annotations and Indonesian validation messages, e.g. `"Nama kategori wajib diisi"`), and `<Entity>ListViewModel` (`Items`, `Page`, `PageSize`, `TotalCount`, computed `TotalPages`).
- Actions: `Index` server-renders the first page; `List` is a JSON endpoint the page's JS polls for pagination without a full reload; `Create`/`Update` return `Json(...)` on success and `BadRequest`/`NotFound` with `{ message }` on failure; `Delete` returns `Ok()`. All mutating POST actions carry `[ValidateAntiForgeryToken]`.
- Frontend: no admin SPA framework or per-view JS files — CRUD logic is written as vanilla JS (`fetch` + Bootstrap modals for Add/Edit, a hidden form + `confirm()` for Delete) directly inside the view's `@section PageScripts` block, re-rendering the table body and pagination client-side after each mutation.
- User-facing strings and validation messages in the Admin area are in Bahasa Indonesia — keep new ones consistent with that.
