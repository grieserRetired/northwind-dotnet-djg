using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindDotNet.Api.Data;
using NorthwindDotNet.Api.Models;

namespace NorthwindDotNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly NorthwindDbContext _context;

    public OrdersController(NorthwindDbContext context) => _context = context;

    // GET: api/Orders
    // Optional filter: api/Orders?companyId=3
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders([FromQuery] int? companyId = null)
    {
        var query = _context.Orders
            .AsNoTracking()
            .Include(o => o.Employee)
            .Include(o => o.Customer)
            .Include(o => o.Shipper)
            .Include(o => o.OrderStatus)
            .Include(o => o.TaxStatus)
            .AsQueryable();

        if (companyId.HasValue)
            query = query.Where(o => o.CustomerId == companyId.Value);

        return await query.ToListAsync();
    }

    // GET: api/Orders/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Employee)
            .Include(o => o.Customer)
            .Include(o => o.Shipper)
            .Include(o => o.OrderStatus)
            .Include(o => o.TaxStatus)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.OrderDetailStatus)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        return order is null ? NotFound() : order;
    }

    // POST: api/Orders
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Order>> PostOrder(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
    }

    // PUT: api/Orders/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutOrder(int id, Order order)
    {
        if (id != order.OrderId)
            return BadRequest();

        order.Employee = null;
        order.Customer = null;
        order.Shipper = null;
        order.OrderStatus = null;
        order.TaxStatus = null;
        order.OrderDetails = [];

        _context.Entry(order).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Orders.AnyAsync(e => e.OrderId == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Orders/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order is null)
            return NotFound();

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
