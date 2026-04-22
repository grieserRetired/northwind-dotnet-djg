using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindDotNet.Api.Data;
using NorthwindDotNet.Api.Models;

namespace NorthwindDotNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrderDetailsController : ControllerBase
{
    private readonly NorthwindDbContext _context;

    public PurchaseOrderDetailsController(NorthwindDbContext context) => _context = context;

    // GET: api/PurchaseOrderDetails
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PurchaseOrderDetail>>> GetPurchaseOrderDetails()
    {
        return await _context.PurchaseOrderDetails
            .AsNoTracking()
            .Include(pod => pod.PurchaseOrder)
            .Include(pod => pod.Product)
            .ToListAsync();
    }

    // GET: api/PurchaseOrderDetails/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PurchaseOrderDetail>> GetPurchaseOrderDetail(int id)
    {
        var purchaseOrderDetail = await _context.PurchaseOrderDetails
            .AsNoTracking()
            .Include(pod => pod.PurchaseOrder)
                .ThenInclude(po => po!.Status)
            .Include(pod => pod.Product)
                .ThenInclude(p => p!.ProductCategory)
            .FirstOrDefaultAsync(pod => pod.PurchaseOrderDetailId == id);

        return purchaseOrderDetail is null ? NotFound() : purchaseOrderDetail;
    }

    // POST: api/PurchaseOrderDetails
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PurchaseOrderDetail>> PostPurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail)
    {
        _context.PurchaseOrderDetails.Add(purchaseOrderDetail);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPurchaseOrderDetail), new { id = purchaseOrderDetail.PurchaseOrderDetailId }, purchaseOrderDetail);
    }

    // PUT: api/PurchaseOrderDetails/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutPurchaseOrderDetail(int id, PurchaseOrderDetail purchaseOrderDetail)
    {
        if (id != purchaseOrderDetail.PurchaseOrderDetailId)
            return BadRequest();

        purchaseOrderDetail.PurchaseOrder = null;
        purchaseOrderDetail.Product = null;

        _context.Entry(purchaseOrderDetail).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.PurchaseOrderDetails.AnyAsync(e => e.PurchaseOrderDetailId == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/PurchaseOrderDetails/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePurchaseOrderDetail(int id)
    {
        var purchaseOrderDetail = await _context.PurchaseOrderDetails.FindAsync(id);
        if (purchaseOrderDetail is null)
            return NotFound();

        _context.PurchaseOrderDetails.Remove(purchaseOrderDetail);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
