using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindDotNet.Api.Data;
using NorthwindDotNet.Api.Models;

namespace NorthwindDotNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly NorthwindDbContext _context;

    public ContactsController(NorthwindDbContext context) => _context = context;

    // GET: api/Contacts
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
    {
        return await _context.Contacts
            .AsNoTracking()
            .Include(c => c.Company)
            .ToListAsync();
    }

    // GET: api/Contacts/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        var contact = await _context.Contacts
            .AsNoTracking()
            .Include(c => c.Company)
                .ThenInclude(co => co!.CompanyType)
            .Include(c => c.Company)
                .ThenInclude(co => co!.State)
            .FirstOrDefaultAsync(c => c.ContactId == id);

        return contact is null ? NotFound() : contact;
    }

    // POST: api/Contacts
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Contact>> PostContact(Contact contact)
    {
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetContact), new { id = contact.ContactId }, contact);
    }

    // PUT: api/Contacts/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutContact(int id, Contact contact)
    {
        if (id != contact.ContactId)
            return BadRequest();

        contact.Company = null;

        _context.Entry(contact).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Contacts.AnyAsync(e => e.ContactId == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Contacts/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact is null)
            return NotFound();

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
