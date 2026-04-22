using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindDotNet.Api.Data;
using NorthwindDotNet.Api.Models;

namespace NorthwindDotNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly NorthwindDbContext _context;

    public CompaniesController(NorthwindDbContext context) => _context = context;

    // GET: api/Companies
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
    {
        return await _context.Companies
            .AsNoTracking()
            .Include(c => c.CompanyType)
            .Include(c => c.State)
            .Include(c => c.StandardTaxStatus)
            .ToListAsync();
    }

    // GET: api/Companies/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Company>> GetCompany(int id)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .Include(c => c.CompanyType)
            .Include(c => c.State)
            .Include(c => c.StandardTaxStatus)
            .Include(c => c.Contacts)
            .FirstOrDefaultAsync(c => c.CompanyId == id);

        return company is null ? NotFound() : company;
    }

    // POST: api/Companies
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Company>> PostCompany(Company company)
    {
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCompany), new { id = company.CompanyId }, company);
    }

    // PUT: api/Companies/5
    // Navigation properties on the request body are ignored; send only scalar
    // properties and FK values (CompanyTypeId, StateAbbrev, StandardTaxStatusId).
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutCompany(int id, Company company)
    {
        if (id != company.CompanyId)
            return BadRequest();

        // Null out navigation properties before attaching to prevent EF Core
        // from attempting to insert/update any nested objects from the request body.
        company.CompanyType = null;
        company.State = null;
        company.StandardTaxStatus = null;
        company.Contacts = [];
        company.CustomerOrders = [];
        company.ShipperOrders = [];
        company.ProductVendors = [];
        company.PurchaseOrders = [];

        _context.Entry(company).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Companies.AnyAsync(e => e.CompanyId == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Companies/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company is null)
            return NotFound();

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
