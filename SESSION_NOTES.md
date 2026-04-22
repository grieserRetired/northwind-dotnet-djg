# Session Notes — Northwind .NET 8 Project

**Last updated:** 2026-04-22  
**Git user:** grieserRetired

---

## What Was Built

This project is a full-stack .NET 8 port of the classic Microsoft Northwind Access sample database. Everything below was built across two sessions.

### Session 1 — API Foundation

| Area | Work Done |
|---|---|
| Database schema | `database/001_initial_schema.sql` — 29-table PostgreSQL schema, FK constraints, CASCADE deletes, indexes |
| Reference seed data | `database/002_seed_reference_data.sql` — lookup tables: CompanyTypes, TaxStatus, OrderStatus, OrderDetailStatus, PurchaseOrderStatus, Privileges, Titles, States |
| C# entity models | 28 model classes in `NorthwindDotNet.Api/Models/`, with `[Table]`, `[Column]`, `[Key]`, `[ForeignKey]`, `[InverseProperty]` attributes |
| DbContext | `NorthwindDotNet.Api/Data/NorthwindDbContext.cs` — all 28 DbSets, Fluent API for composite unique constraint and 4 cascade deletes |
| CRUD controllers | 8 controllers for all business-facing entities: Companies, Contacts, Employees, Products, Orders, OrderDetails, PurchaseOrders, PurchaseOrderDetails |

### Session 2 — Transactional Data + Razor Pages Frontend

| Area | Work Done |
|---|---|
| Transactional seed data | `database/003_seed_transactional_data.sql` — 5 categories, 15 companies, 5 employees, 20 products, 8 purchase orders (18 detail lines), 15 orders (37 detail lines) |
| Razor Pages | 4 pages in `NorthwindDotNet.Web/Pages/`: Orders list, Orders detail, Products list, Companies list |
| DTOs | 4 DTO files in `NorthwindDotNet.Web/Models/` — no reference to the Api project, all deserialized from HTTP |
| HttpClient | Named client `"NorthwindApi"` registered in `NorthwindDotNet.Web/Program.cs` |
| Navigation | `_Layout.cshtml` updated with nav links to all 4 pages |

---

## Current Project State

### Build Status

Both projects build clean:

```
dotnet build NorthwindDotNet.Api/NorthwindDotNet.Api.csproj   → 0 warnings, 0 errors
dotnet build NorthwindDotNet.Web/NorthwindDotNet.Web.csproj   → 0 warnings, 0 errors
```

### Git Log

```
45f106d Add Razor Pages frontend - Orders, Products, Companies with bootstrap
bd1eb70 Add transactional seed data - 15 orders, 20 products, 15 companies
5fd426c Add CRUD API controllers for core business entities
a7eea6a Add C# entity models for all 28 Northwind tables
e50ed47 Add PostgreSQL schema and reference data seed scripts
ec4aff9 Add .gitignore
d2e3199 initial solution scaffold
```

### Project Layout

```
northwind-dotnet-djg/
├── database/
│   ├── 001_initial_schema.sql          # Full 29-table schema
│   ├── 002_seed_reference_data.sql     # Lookup/enum tables
│   └── 003_seed_transactional_data.sql # Business data (run this to get a working dataset)
├── NorthwindDotNet.Api/
│   ├── Controllers/                    # 8 CRUD controllers
│   ├── Data/NorthwindDbContext.cs
│   ├── Models/                         # 28 entity classes
│   ├── appsettings.json                # DB connection string here
│   └── Properties/launchSettings.json  # Runs on http://localhost:5132
└── NorthwindDotNet.Web/
    ├── Models/                         # 4 DTO files (no Api project ref)
    ├── Pages/
    │   ├── Orders/Index.cshtml(.cs)
    │   ├── Orders/Details.cshtml(.cs)  # Route: /Orders/Details/{id}
    │   ├── Products/Index.cshtml(.cs)
    │   └── Companies/Index.cshtml(.cs)
    ├── Pages/Shared/_Layout.cshtml     # Shared nav bar
    └── Properties/launchSettings.json  # Runs on http://localhost:5178
```

---

## Database Connection

**Engine:** PostgreSQL (local)  
**Database name:** `northwind`  
**Username:** `postgres`  
**Password:** `changeme`  
**Connection string** (in `NorthwindDotNet.Api/appsettings.json`):

```
Host=localhost;Database=northwind;Username=postgres;Password=changeme
```

### Setting Up the Database from Scratch

Run scripts in order:

```bash
psql -U postgres -c "CREATE DATABASE northwind;"
psql -U postgres -d northwind -f database/001_initial_schema.sql
psql -U postgres -d northwind -f database/002_seed_reference_data.sql
psql -U postgres -d northwind -f database/003_seed_transactional_data.sql
```

Each script is idempotent-friendly via `BEGIN/COMMIT` transactions. If a script fails partway through, the whole script rolls back cleanly.

### Running the Apps

Start API first, then Web:

```bash
# Terminal 1
dotnet run --project NorthwindDotNet.Api --launch-profile http

# Terminal 2
dotnet run --project NorthwindDotNet.Web --launch-profile http
```

- API: http://localhost:5132
- Web: http://localhost:5178

---

## Important Technical Decisions

### API Project

- **No EF Migrations.** Schema is managed entirely through the `database/` SQL scripts. This matches the original Access-first workflow and keeps the SQL human-readable.

- **`AsNoTracking()` on all GET queries.** Prevents EF from accumulating tracked entities and avoids needing to detach before returning JSON.

- **`ReferenceHandler.IgnoreCycles` for JSON serialization.** Navigation properties create reference cycles (Order → OrderDetails → Order). Cycles are silently broken rather than throwing.

- **PUT pattern — null out navigation properties before attach.** All PUT controllers set every navigation property to null (and collections to `[]`) before calling `_context.Entry(entity).State = EntityState.Modified`. This prevents EF from trying to insert/update nested objects sent on the request body.

- **Two FKs to the same table.** `Orders` has both `CustomerID` and `ShipperID` → `Companies`. `PurchaseOrders` has both `SubmittedByID` and `ApprovedByID` → `Employees`. These are disambiguated with `[InverseProperty]` on both sides.

- **Cascade deletes explicit in Fluent API.** EF Core defaults optional FKs to `ClientSetNull`. Four relationships that have `ON DELETE CASCADE` in the SQL schema (EmployeePrivileges, Mru, OrderDetails, PurchaseOrderDetails) are explicitly set to `DeleteBehavior.Cascade` in `OnModelCreating`.

- **Composite unique constraint via Fluent API.** The `Companies(CompanyName, CompanyTypeID)` unique index cannot be expressed through data annotations; it lives in `OnModelCreating`.

### Seed Data

- **Explicit SERIAL IDs + `setval()`.** Every `INSERT` specifies the PK value explicitly, and `setval(pg_get_serial_sequence(...), MAX(id))` is called after each table so the sequence is correct for future application inserts.

- **FK insertion order.** Strictly: ProductCategories → Companies → Employees → EmployeePrivileges → Products → ProductVendors → Contacts → Orders → OrderDetails → PurchaseOrders → PurchaseOrderDetails.

- **PO approval authorization.** Only employees 1 (Andrew Fuller) and 4 (Steven Buchanan) have an `EmployeePrivileges` row for `PrivilegeID = 1` (Approve Purchase Orders). All approved POs in the seed data use only these two employees.

- **Status consistency.** `OrderDetail.OrderDetailStatusID` is chosen to be semantically consistent with the parent `Order.OrderStatusID` (e.g., Shipped orders have all lines in Shipped status), making the seed data usable for lifecycle query testing.

### Web Project

- **No project reference to Api.** The Web project has standalone DTO classes in `NorthwindDotNet.Web/Models/`. This keeps the projects independently deployable and avoids shipping EF Core into the frontend.

- **Named HttpClient.** `IHttpClientFactory` is used with a named client `"NorthwindApi"` (base address `http://localhost:5132`). Page models inject `IHttpClientFactory` and call `CreateClient("NorthwindApi")`.

- **Case-insensitive JSON deserialization.** The API serializes with default `PropertyNamingPolicy = CamelCase` (ASP.NET Core controller default). `HttpClient.GetFromJsonAsync<T>` uses `JsonSerializerDefaults.Web`, which sets `PropertyNameCaseInsensitive = true`, so the PascalCase DTO properties deserialize from camelCase JSON without any configuration.

- **No `UnitsInStock` column.** The `Product` model does not have a stock quantity field. The Products page shows `ReorderLevel` as the practical equivalent. A `StockTakes` table exists in the schema for inventory snapshots but has no seed data.

---

## Known Issues / Limitations

| # | Issue | Impact |
|---|---|---|
| 1 | **Password in appsettings.json** — `Password=changeme` is committed to source. | Low risk locally; must use User Secrets or env vars before any deployment. |
| 2 | **No Swagger/OpenAPI** — The API has no documentation endpoint. | Makes manual exploration harder. Easy to add. |
| 3 | **No error page for API-down** — If the API isn't running, the Web pages show a plain `alert-danger` div with the exception message. | Acceptable for dev; needs proper error UX for production. |
| 4 | **`StockTakes` table is empty** — The schema and model exist but no seed data was added. | Products page shows "Reorder Level" instead of live stock counts. |
| 5 | **No authentication** — All endpoints are open, all pages are anonymous. | Fine for local dev sample; would need ASP.NET Core Identity or similar for real use. |
| 6 | **List endpoints load full object graphs** — e.g., `GET /api/Companies` includes `CompanyType`, `State`, and `StandardTaxStatus` for every row. | Acceptable for 15 companies; could become slow at scale. Would benefit from projection/DTOs on the API side. |

---

## Suggested Next Steps

### High Value / Low Effort

- **Add Swagger** — one NuGet package (`Swashbuckle.AspNetCore`) and two lines in `Program.cs`. Makes the API self-documenting and easy to test without a client.
- **Move DB password to User Secrets** — `dotnet user-secrets set "ConnectionStrings:DefaultConnection" "..."` to keep credentials out of source.
- **Add `appsettings.Development.json` override in Web** — externalize `http://localhost:5132` into configuration instead of hardcoding in `Program.cs`.

### Functional Additions

- **Orders/Details → Contacts page** — Add `/Contacts` list page with filtering by company type (customer vs. vendor).
- **Employees page** — `/Employees` list with supervisor relationship shown.
- **Purchase Orders pages** — `/PurchaseOrders` list and detail page (mirrors the Orders pages already built).
- **Company detail page** — drill-down from the Companies list showing contacts, customer orders, and purchase orders for that company.
- **Sorting and filtering** — The list pages currently sort client-side in Razor. Adding query-string parameters and passing them to the API would allow server-side sorting and filtering on large datasets.

### API Improvements

- **Pagination** — All list endpoints return every row. Add `?page=1&pageSize=25` query parameters.
- **Search/filter endpoints** — e.g., `GET /api/orders?customerId=2&statusId=3`.
- **API-side DTOs / projections** — Return only the columns each client actually needs rather than the full entity graph.
- **Validation** — Add `[Required]` / `[Range]` data annotations to models and enable `ModelState` validation responses on POST/PUT.
