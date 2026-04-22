using Microsoft.EntityFrameworkCore;
using NorthwindDotNet.Api.Models;

namespace NorthwindDotNet.Api.Data;

public class NorthwindDbContext : DbContext
{
    public NorthwindDbContext(DbContextOptions<NorthwindDbContext> options) : base(options) { }

    // -------------------------------------------------------------------------
    // Tier 1 – lookup / reference tables
    // -------------------------------------------------------------------------
    public DbSet<State> States => Set<State>();
    public DbSet<CompanyType> CompanyTypes => Set<CompanyType>();
    public DbSet<TaxStatus> TaxStatuses => Set<TaxStatus>();
    public DbSet<Title> Titles => Set<Title>();
    public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
    public DbSet<OrderDetailStatus> OrderDetailStatuses => Set<OrderDetailStatus>();
    public DbSet<PurchaseOrderStatus> PurchaseOrderStatuses => Set<PurchaseOrderStatus>();
    public DbSet<Privilege> Privileges => Set<Privilege>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();

    // -------------------------------------------------------------------------
    // Tier 1 – application metadata / UI tables
    // -------------------------------------------------------------------------
    public DbSet<CatalogTableOfContents> CatalogTableOfContents => Set<CatalogTableOfContents>();
    public DbSet<Learn> Learns => Set<Learn>();
    public DbSet<NorthwindFeature> NorthwindFeatures => Set<NorthwindFeature>();
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();
    public DbSet<UserSetting> UserSettings => Set<UserSetting>();
    public DbSet<USysRibbon> USysRibbons => Set<USysRibbon>();
    public DbSet<Welcome> Welcomes => Set<Welcome>();
    public DbSet<NorthwindString> Strings => Set<NorthwindString>();

    // -------------------------------------------------------------------------
    // Tier 2 – core business tables
    // -------------------------------------------------------------------------
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<EmployeePrivilege> EmployeePrivileges => Set<EmployeePrivilege>();
    public DbSet<Mru> MruEntries => Set<Mru>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVendor> ProductVendors => Set<ProductVendor>();
    public DbSet<StockTake> StockTakes => Set<StockTake>();

    // -------------------------------------------------------------------------
    // Tier 3 – orders & purchase orders
    // -------------------------------------------------------------------------
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderDetail> PurchaseOrderDetails => Set<PurchaseOrderDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---------------------------------------------------------------------
        // Companies – composite unique constraint (CompanyName, CompanyTypeID).
        // Mirrors the original Access "CustomerName" index. Cannot be expressed
        // through a single data annotation.
        // ---------------------------------------------------------------------
        modelBuilder.Entity<Company>()
            .HasIndex(c => new { c.CompanyName, c.CompanyTypeId })
            .IsUnique()
            .HasDatabaseName("UQ_Companies_CustomerName");

        // ---------------------------------------------------------------------
        // Cascade deletes – EF Core defaults to ClientSetNull for optional
        // (nullable FK) relationships, which does not match the four explicit
        // ON DELETE CASCADE constraints in 001_initial_schema.sql.
        // ---------------------------------------------------------------------

        modelBuilder.Entity<EmployeePrivilege>()
            .HasOne(ep => ep.Employee)
            .WithMany(e => e.EmployeePrivileges)
            .HasForeignKey(ep => ep.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Mru>()
            .HasOne(m => m.Employee)
            .WithMany(e => e.MruEntries)
            .HasForeignKey(m => m.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PurchaseOrderDetail>()
            .HasOne(pod => pod.PurchaseOrder)
            .WithMany(po => po.PurchaseOrderDetails)
            .HasForeignKey(pod => pod.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
