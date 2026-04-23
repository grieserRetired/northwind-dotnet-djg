Northwind Migration — Session Handoff Notes
Date: April 23, 2026
Project: northwind-dotnet-djg
GitHub: https://github.com/grieserRetired/northwind-dotnet-djg

---

## Live URLs
- **API:** https://northwind-dotnet-djg-production.up.railway.app
- **Web:** https://happy-upliftment-production-0c10.up.railway.app

---

## What Has Been Built

### Infrastructure
- VirtualBox + Windows 10 VM on Ubuntu host
- Microsoft 365 trial (Access) installed in VM
- Northwind Developer Edition exported via MSAccess-VCS add-in
- Export located at: ~/NorthwindExport/NWDevEdition.accdb.src/
- PostgreSQL 16 installed and running on Ubuntu
- .NET 8 SDK installed
- pgAdmin 4 installed — connected to both local and Railway databases

### Database
- PostgreSQL database: `northwind` (local) / `railway` (Railway)
- User: `northwind_user` / Password: `northwind123` (local)
- Railway public endpoint: `shortline.proxy.rlwy.net:34749`
- Scripts in ~/northwind-dotnet-djg/database/:
  - 001_initial_schema.sql — 28 tables, all FKs, indexes
  - 002_seed_reference_data.sql — lookup tables (OrderStatus, States, etc.)
  - 003_seed_transactional_data.sql — 15 companies, 5 employees, 20 products, 15 orders

### .NET Solution

**NorthwindDotNet.Api** — Web API deployed to Railway
- 8 CRUD controllers: Companies, Contacts, Products, Orders, OrderDetails,
  Employees, PurchaseOrders, PurchaseOrderDetails
- ReportsController with sales-by-employee and sales-by-product endpoints
- Orders controller supports ?companyId= filter
- EF Core DbContext with Npgsql
- Connection string via user-secrets (local) / Railway environment variables (prod)

**NorthwindDotNet.Web** — Razor Pages deployed to Railway
- /Orders — list with color-coded status badges
- /Orders/Details/{id} — line items, totals, shipping, Print Invoice button
- /Products — list with category, price, reorder level (clickable to detail)
- /Products/Details/{id} — full product info + vendor list
- /Companies — list with color-coded type badges (clickable to detail)
- /Companies/Details/{id} — contacts table + order history
- /Reports — landing page with links to all reports
- /Reports/Invoice — printable invoice, accessible by order number or from order detail
- /Reports/SalesByEmployee — date range filter, revenue summary by employee
- /Reports/SalesByProduct — date range filter, units and revenue by product
- All reports have Print / Save as PDF button (browser native)

---

## To Resume Development

Start the API:
```
cd ~/northwind-dotnet-djg/NorthwindDotNet.Api
dotnet run
```

Start the Web App:
```
cd ~/northwind-dotnet-djg/NorthwindDotNet.Web
dotnet run
```

Start Claude Code:
```
cd ~/northwind-dotnet-djg
claude
```

---

## Reference Files in Repo
- ACCESS_MIGRATION_GUIDE.md — naming conventions, data type mappings,
  gotchas, EF Core config for PostgreSQL (snake_case vs PascalCase)

---

## Architecture Decisions Made
- Web project uses its own DTOs (not shared with API) — clean separation
- PostgreSQL chosen over SQL Server — free, Railway/Render compatible
- Tables use PascalCase (inherited from Access) — requires quoting in raw SQL
  (SELECT * FROM "Orders") but EF Core handles this transparently
- Reports built as Razor Pages with print CSS — no SSRS, no separate tool
- Deployment: Railway free tier, auto-deploy on git push

---

## Known Issues / TODOs
- HTTPS warning on API startup (harmless for local dev)
- Guest Additions not installed in Windows VM (file transfer via email workaround)
- ACCESS_MIGRATION_GUIDE.md generated as chat artifact — needs to be committed to repo

---

## Next Project: Real-World Migration

After Northwind, the plan is to migrate a more complex, real-world Access database
and deploy it as a public-facing multi-user SaaS application.

Goals:
- Pick a source database with messier real-world characteristics
  (missing FKs, inconsistent naming, implicit lookups, denormalized data)
- Public-facing: anyone can register and create a test account
- ASP.NET Identity for user authentication
- Row-level data isolation (each user sees only their own data)
- "Seed my account with sample data" button for visitors
- Portfolio write-up: "From legacy Access to deployed SaaS in X hours with Claude Code"

Target database characteristics to look for:
- Recurring billing, membership, or AR tracking (more business logic than Northwind)
- Some normalization issues to solve (demonstrates real migration skill)
- Enough complexity to be impressive, not so much it becomes a slog

---

Last updated: April 23, 2026 — Session 2
