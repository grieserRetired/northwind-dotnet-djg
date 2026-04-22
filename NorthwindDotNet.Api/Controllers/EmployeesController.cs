using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindDotNet.Api.Data;
using NorthwindDotNet.Api.Models;

namespace NorthwindDotNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly NorthwindDbContext _context;

    public EmployeesController(NorthwindDbContext context) => _context = context;

    // GET: api/Employees
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        return await _context.Employees
            .AsNoTracking()
            .Include(e => e.TitleNavigation)
            .Include(e => e.Supervisor)
            .ToListAsync();
    }

    // GET: api/Employees/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var employee = await _context.Employees
            .AsNoTracking()
            .Include(e => e.TitleNavigation)
            .Include(e => e.Supervisor)
            .Include(e => e.Subordinates)
            .Include(e => e.EmployeePrivileges)
                .ThenInclude(ep => ep.Privilege)
            .FirstOrDefaultAsync(e => e.EmployeeId == id);

        return employee is null ? NotFound() : employee;
    }

    // POST: api/Employees
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
    }

    // PUT: api/Employees/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutEmployee(int id, Employee employee)
    {
        if (id != employee.EmployeeId)
            return BadRequest();

        employee.TitleNavigation = null;
        employee.Supervisor = null;
        employee.Subordinates = [];
        employee.EmployeePrivileges = [];
        employee.MruEntries = [];
        employee.Orders = [];
        employee.SubmittedPurchaseOrders = [];
        employee.ApprovedPurchaseOrders = [];

        _context.Entry(employee).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Employees.AnyAsync(e => e.EmployeeId == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Employees/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee is null)
            return NotFound();

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
