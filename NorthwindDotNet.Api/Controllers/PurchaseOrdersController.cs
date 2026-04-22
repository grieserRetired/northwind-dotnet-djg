using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindDotNet.Api.Data;
using NorthwindDotNet.Api.Models;

namespace NorthwindDotNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrdersController : ControllerBase
{
    private readonly NorthwindDbContext _context;

    public PurchaseOrdersController(NorthwindDbContext context) => _context = context;

    // GET: api/PurchaseOrders
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetPurchaseOrders()
    {
        return await _context.PurchaseOrders
            .AsNoTracking()
            .Include(po => po.Vendor)
            .Include(po => po.SubmittedBy)
            .Include(po => po.ApprovedBy)
            .Include(po => po.Status)
            .ToListAsync();
    }

    // GET: api/PurchaseOrders/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrder(int id)
    {
        var purchaseOrder = await _context.PurchaseOrders
            .AsNoTracking()
            .Include(po => po.Vendor)
            .Include(po => po.SubmittedBy)
            .Include(po => po.ApprovedBy)
            .Include(po => po.Status)
            .Include(po => po.PurchaseOrderDetails)
                .ThenInclude(pod => pod.Product)
            .FirstOrDefaultAsync(po => po.PurchaseOrderId == id);

        return purchaseOrder is null ? NotFound() : purchaseOrder;
    }

    // POST: api/PurchaseOrders
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PurchaseOrder>> PostPurchaseOrder(PurchaseOrder purchaseOrder)
    {
        _context.PurchaseOrders.Add(purchaseOrder);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPurchaseOrder), new { id = purchaseOrder.PurchaseOrderId }, purchaseOrder);
    }

    // PUT: api/PurchaseOrders/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutPurchaseOrder(int id, PurchaseOrder purchaseOrder)
    {
        if (id != purchaseOrder.PurchaseOrderId)
            return BadRequest();

        purchaseOrder.Vendor = null;
        purchaseOrder.SubmittedBy = null;
        purchaseOrder.ApprovedBy = null;
        purchaseOrder.Status = null;
        purchaseOrder.PurchaseOrderDetails = [];

        _context.Entry(purchaseOrder).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.PurchaseOrders.AnyAsync(e => e.PurchaseOrderId == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/PurchaseOrders/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePurchaseOrder(int id)
    {
        var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
        if (purchaseOrder is null)
            return NotFound();

        _context.PurchaseOrders.Remove(purchaseOrder);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
