# Northwind Migration — Session Handoff Notes
**Date:** April 22, 2026  
**Project:** northwind-dotnet-djg  
**GitHub:** https://github.com/grieserRetired/northwind-dotnet-djg

---

## What Was Built Today

### Infrastructure
- VirtualBox + Windows 10 VM on Ubuntu host
- Microsoft 365 trial (Access) installed in VM
- Northwind Developer Edition exported via MSAccess-VCS add-in
- Export located at: ~/NorthwindExport/NWDevEdition.accdb.src/
- PostgreSQL 16 installed and running on Ubuntu
- .NET 8 SDK installed

### Database
- PostgreSQL database: `northwind`
- User: `northwind_user` / Password: `northwind123`
- Host: localhost
- Scripts in ~/northwind-dotnet-djg/database/:
  - 001_initial_schema.sql — 28 tables, all FKs, indexes
  - 002_seed_reference_data.sql — lookup tables (OrderStatus, States, etc.)
  - 003_seed_transactional_data.sql — 15 companies, 5 employees, 20 products, 15 orders

### .NET Solution
- NorthwindDotNet.Api — Web API on http://localhost:5132
  - 8 CRUD controllers (Companies, Contacts, Products, Orders, OrderDetails, Employees, PurchaseOrders, PurchaseOrderDetails)
  - EF Core DbContext with Npgsql
  - Connection string via user-secrets
- NorthwindDotNet.Web — Razor Pages on http://localhost:5178
  - /Orders — list with color-coded status badges
  - /Orders/Details/{id} — line items, totals, shipping
  - /Products — list with category, price, reorder level
  - /Companies — list with color-coded type badges

---

## To Resume Development

### Start the API
```bash
cd ~/northwind-dotnet-djg/NorthwindDotNet.Api
dotnet run
```

### Start the Web App
```bash
cd ~/northwind-dotnet-djg/NorthwindDotNet.Web
dotnet run
```

### Start Claude Code
```bash
cd ~/northwind-dotnet-djg
claude
```

---

## Next Session Plan

1. **Deploy to Railway** — get a public URL, configure PostgreSQL on Railway
2. **Reports section** — Invoice, Sales by Employee, Sales by Product (Razor Pages + print CSS)
3. **Product detail page** — vendors, order history
4. **Company detail page** — contacts, order history  
5. **Install pgAdmin** — GUI database browser (like SSMS for PostgreSQL)

---

## Known Issues / TODOs
- Products and Companies have no detail pages yet
- HTTPS warning on API startup (harmless for local dev, fix for deployment)
- Guest Additions not installed in Windows VM (file transfer via email workaround)
- VM password for Windows: set during install
- Ubuntu sudo password: machine login password

---

## Architecture Decisions Made
- Web project uses its own DTOs (not shared with API) — clean separation
- PostgreSQL chosen over SQL Server — free, Railway/Render compatible
- Reports will be Razor Pages with print CSS + QuestPDF for complex layouts
- Deployment target: Railway (free tier, auto-deploy on git push)

