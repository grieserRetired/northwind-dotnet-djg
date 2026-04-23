using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindDotNet.Api.Data;

namespace NorthwindDotNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly NorthwindDbContext _context;
    public ReportsController(NorthwindDbContext context) => _context = context;

    // GET: api/reports/sales-by-employee?from=2025-01-01&to=2025-12-31
    [HttpGet("sales-by-employee")]
    public async Task<IActionResult> SalesByEmployee(
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null)
    {
        var query = _context.Orders
            .AsNoTracking()
            .Include(o => o.Employee)
            .Include(o => o.OrderDetails)
            .Where(o => o.OrderDate.HasValue);

        if (from.HasValue) query = query.Where(o => o.OrderDate >= from.Value);
        if (to.HasValue)   query = query.Where(o => o.OrderDate <= to.Value);

        var orders = await query.ToListAsync();

        var result = orders
            .GroupBy(o => new {
                o.EmployeeId,
                FirstName = o.Employee?.FirstName ?? "Unknown",
                LastName  = o.Employee?.LastName  ?? ""
            })
            .Select(g => new
            {
                EmployeeId   = g.Key.EmployeeId,
                EmployeeName = $"{g.Key.FirstName} {g.Key.LastName}".Trim(),
                OrderCount   = g.Count(),
                TotalRevenue = g.Sum(o =>
                    o.OrderDetails.Sum(d =>
                        (d.UnitPrice ?? 0m) * (d.Quantity ?? 0) *
                        (1m - (decimal)(d.Discount ?? 0f))
                    ) + (o.ShippingFee ?? 0m)
                )
            })
            .OrderByDescending(x => x.TotalRevenue)
            .ToList();

        return Ok(result);
    }

    // GET: api/reports/sales-by-product?from=2025-01-01&to=2025-12-31
    [HttpGet("sales-by-product")]
    public async Task<IActionResult> SalesByProduct(
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null)
    {
        var query = _context.OrderDetails
            .AsNoTracking()
            .Include(od => od.Product)
                .ThenInclude(p => p!.ProductCategory)
            .Include(od => od.Order)
            .Where(od => od.Order!.OrderDate.HasValue);

        if (from.HasValue) query = query.Where(od => od.Order!.OrderDate >= from.Value);
        if (to.HasValue)   query = query.Where(od => od.Order!.OrderDate <= to.Value);

        var details = await query.ToListAsync();

        var result = details
            .GroupBy(od => new {
                od.ProductId,
                ProductName  = od.Product?.ProductName  ?? "Unknown",
                CategoryName = od.Product?.ProductCategory?.ProductCategoryName ?? "—"
            })
            .Select(g => new
            {
                ProductId    = g.Key.ProductId,
                ProductName  = g.Key.ProductName,
                CategoryName = g.Key.CategoryName,
                UnitsSold    = g.Sum(od => od.Quantity ?? 0),
                TotalRevenue = g.Sum(od =>
                    (od.UnitPrice ?? 0m) * (od.Quantity ?? 0) *
                    (1m - (decimal)(od.Discount ?? 0f))
                )
            })
            .OrderByDescending(x => x.TotalRevenue)
            .ToList();

        return Ok(result);
    }
}
