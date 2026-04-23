# MS Access Migration — Best Practices & Conventions

A living reference for migrating MS Access databases to modern platforms.
Update this as you learn new lessons.

---

## Naming Conventions by Target Database

This is one of the most impactful decisions — make it before you write a single table.
Access itself is case-insensitive and uses no consistent convention, so you must choose.

| Target | Tables | Columns | Notes |
|---|---|---|---|
| **SQL Server** | `PascalCase` | `PascalCase` | Native convention; SSMS, EF Core all expect this |
| **PostgreSQL** | `snake_case` | `snake_case` | Unquoted identifiers are lowercased by the engine; PascalCase requires quoting everywhere |
| **MySQL** | `snake_case` | `snake_case` | Table names are case-sensitive on Linux (file system); snake_case avoids surprises |
| **SQLite** | Either | Either | Case-insensitive; snake_case preferred by convention |

### The PostgreSQL Quoting Problem
PostgreSQL folds all unquoted identifiers to lowercase at parse time.
If you create a table as `"Orders"` (quoted), you must quote it in **every query forever**:

```sql
-- Works (quoted)
SELECT * FROM "Orders" WHERE "OrderId" = 1;

-- Fails — PostgreSQL looks for a table called "orders"
SELECT * FROM Orders WHERE OrderId = 1;
```

**Recommendation:** When targeting PostgreSQL, convert all Access object names to `snake_case`
at migration time. Your queries will be cleaner and every PostgreSQL tool will work naturally.

```
Access:      tblOrders, OrderID, ShipDate
PostgreSQL:  orders,    order_id, ship_date
```

---

## Data Type Mappings

### Access → SQL Server
| Access | SQL Server |
|---|---|
| AutoNumber | `INT IDENTITY(1,1)` or `BIGINT IDENTITY` |
| Text (short) | `NVARCHAR(n)` |
| Memo / Long Text | `NVARCHAR(MAX)` |
| Number (Integer) | `INT` |
| Number (Long Integer) | `BIGINT` |
| Number (Double) | `FLOAT` |
| Currency | `DECIMAL(19,4)` |
| Date/Time | `DATETIME2` (prefer over `DATETIME`) |
| Yes/No | `BIT` |
| OLE Object | `VARBINARY(MAX)` |
| Hyperlink | `NVARCHAR(MAX)` |

### Access → PostgreSQL
| Access | PostgreSQL |
|---|---|
| AutoNumber | `SERIAL` or `BIGSERIAL` (or `GENERATED ALWAYS AS IDENTITY`) |
| Text (short) | `VARCHAR(n)` or `TEXT` |
| Memo / Long Text | `TEXT` |
| Number (Integer) | `INTEGER` |
| Number (Long Integer) | `BIGINT` |
| Number (Double) | `DOUBLE PRECISION` |
| Currency | `NUMERIC(19,4)` |
| Date/Time | `TIMESTAMP` or `TIMESTAMPTZ` |
| Yes/No | `BOOLEAN` |
| OLE Object | `BYTEA` |
| Hyperlink | `TEXT` |

### Access → MySQL
| Access | MySQL |
|---|---|
| AutoNumber | `INT AUTO_INCREMENT` |
| Text (short) | `VARCHAR(n)` |
| Memo / Long Text | `TEXT` or `LONGTEXT` |
| Number (Integer) | `INT` |
| Currency | `DECIMAL(19,4)` |
| Date/Time | `DATETIME` |
| Yes/No | `TINYINT(1)` or `BOOLEAN` |

---

## Primary Keys

Access AutoNumber → prefer surrogate integer PKs on all platforms.
Avoid using Access's GUID-style AutoNumber unless you have a specific reason
(GUIDs hurt index performance and readability).

```sql
-- PostgreSQL preferred
id BIGSERIAL PRIMARY KEY

-- SQL Server preferred  
Id INT IDENTITY(1,1) PRIMARY KEY

-- MySQL preferred
id INT NOT NULL AUTO_INCREMENT PRIMARY KEY
```

---

## Access-Specific Gotchas

### Reserved Words
Access allows object names that are reserved words in standard SQL.
Common offenders: `Date`, `Name`, `Status`, `Order`, `User`, `Password`, `Level`, `Group`.
Rename these during migration or you'll fight quoting issues forever.

### Lookup Fields / Value Lists
Access stores lookup field definitions in the table itself (a UI convenience).
On migration, you must decide: inline `CHECK` constraint, or a proper lookup table.
**Prefer lookup tables** — they're reportable, extensible, and indexable.

### Multi-Value Fields
Access supports multi-value fields (a field that holds a list).
These have **no direct equivalent** in relational databases.
Migration options:
- Junction table (correct relational approach)
- Comma-separated string in TEXT column (quick but dirty, avoid if possible)

### Calculated Fields
Access table-level calculated fields should become:
- A view or computed column in SQL Server
- A generated column or view in PostgreSQL/MySQL
- Or just computed in application code

### Relationships and Enforce Referential Integrity
Access lets you define relationships with or without enforcement.
On migration, add proper `FOREIGN KEY` constraints — don't skip this.
Access databases often have "soft" relationships that were never enforced.

---

## EF Core + .NET Conventions

If you're using Entity Framework Core with the migrated database:

### SQL Server (PascalCase — no configuration needed)
EF Core's default conventions match SQL Server PascalCase out of the box.

### PostgreSQL (snake_case — use Npgsql conventions)
Add this to your `DbContext` to avoid quoting every property:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Automatically maps PascalCase C# names to snake_case PostgreSQL names
    modelBuilder.UseSnakeCaseNamingConvention();
}
```

Requires the `EFCore.NamingConventions` NuGet package:
```
dotnet add package EFCore.NamingConventions
```

This means your C# models stay PascalCase (idiomatic C#) and EF Core
handles the translation to snake_case automatically.

---

## Migration Checklist

- [ ] Choose naming convention based on target database (see table above)
- [ ] Map all Access data types to target equivalents
- [ ] Rename reserved words in table/column names
- [ ] Replace multi-value fields with junction tables
- [ ] Replace calculated fields with views or app logic
- [ ] Add proper FK constraints (don't rely on Access relationship definitions)
- [ ] Replace Access lookup fields with lookup tables
- [ ] Seed lookup/reference data separately from transactional data
- [ ] Test all queries — watch for case sensitivity issues (PostgreSQL/MySQL on Linux)
- [ ] If using EF Core + PostgreSQL, add `UseSnakeCaseNamingConvention()`

---

## Lessons Learned (Project-Specific)

### Northwind (April 2026) — Access → PostgreSQL + .NET 8

- Schema was generated with PascalCase table/column names (matching Access source)
- This requires quoting in every raw SQL query in pgAdmin: `SELECT * FROM "Orders"`
- EF Core handles this transparently via the DbContext, so app code is unaffected
- **Next time:** use snake_case from the start when targeting PostgreSQL
- Railway deployment works well for PostgreSQL + .NET — free tier sufficient for dev/demo
- MSAccess-VCS add-in is the best way to export Access source for inspection

---

*Last updated: April 2026*
