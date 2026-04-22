using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindDotNet.Api.Data;
using NorthwindDotNet.Api.Models;

namespace NorthwindDotNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderDetailsController : ControllerBase
{
    private readonly NorthwindDbContext _context;

    public OrderDetailsController(NorthwindDbContext context) => _context = context;

    // GET: api/OrderDetails
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails()
    {
        return await _context.OrderDetails
            .AsNoTracking()
            .Include(od => od.Order)
            .Include(od => od.Product)
            .Include(od => od.OrderDetailStatus)
            .ToListAsync();
    }

    // GET: api/OrderDetails/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDetail>> GetOrderDetail(int id)
    {
        var orderDetail = await _context.OrderDetails
            .AsNoTracking()
            .Include(od => od.Order)
            .Include(od => od.Product)
                .ThenInclude(p => p!.ProductCategory)
            .Include(od => od.OrderDetailStatus)
            .FirstOrDefaultAsync(od => od.OrderDetailId == id);

        return orderDetail is null ? NotFound() : orderDetail;
    }

    // POST: api/OrderDetails
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDetail>> PostOrderDetail(OrderDetail orderDetail)
    {
        _context.OrderDetails.Add(orderDetail);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOrderDetail), new { id = orderDetail.OrderDetailId }, orderDetail);
    }

    // PUT: api/OrderDetails/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutOrderDetail(int id, OrderDetail orderDetail)
    {
        if (id != orderDetail.OrderDetailId)
            return BadRequest();

        orderDetail.Order = null;
        orderDetail.Product = null;
        orderDetail.OrderDetailStatus = null;

        _context.Entry(orderDetail).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.OrderDetails.AnyAsync(e => e.OrderDetailId == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/OrderDetails/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrderDetail(int id)
    {
        var orderDetail = await _context.OrderDetails.FindAsync(id);
        if (orderDetail is null)
            return NotFound();

        _context.OrderDetails.Remove(orderDetail);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
