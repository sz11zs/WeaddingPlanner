using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.API.Models;
using WeddingPlanner.Data;
using WeddingPlanner.Models;

namespace WeddingPlanner.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PartnersController : ControllerBase
{
    private readonly AppDbContext _context;

    public PartnersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Partners
    // Za čitanje partnera potreban je scope read:partners
    [Authorize(Policy = "ReadPartners")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PartnerDto>>> GetAll()
    {
        var partners = await _context.Partners
            .Include(p => p.Category)
            .Select(p => new PartnerDto
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                Phone = p.Phone,
                Email = p.Email,
                CommissionPct = p.CommissionPct,
                CategoryName = p.Category.Name
            })
            .ToListAsync();

        return Ok(partners);
    }

    // GET: api/Partners/5
    // Za čitanje jednog partnera potreban je scope read:partners
    [Authorize(Policy = "ReadPartners")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Partner>> GetById(int id)
    {
        var partner = await _context.Partners
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (partner == null)
            return NotFound();

        return Ok(partner);
    }

    // POST: api/Partners
    // Za dodavanje partnera potreban je scope write:partners
    [Authorize(Policy = "WritePartners")]
    [HttpPost]
    public async Task<ActionResult<Partner>> Create(Partner partner)
    {
        _context.Partners.Add(partner);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = partner.Id },
            partner);
    }

    // DELETE: api/Partners/5
    // Za brisanje partnera potreban je scope write:partners
    [Authorize(Policy = "WritePartners")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var partner = await _context.Partners.FindAsync(id);

        if (partner == null)
            return NotFound();

        _context.Partners.Remove(partner);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // PUT: api/Partners/5
    // Za uređivanje partnera potreban je scope write:partners
    [Authorize(Policy = "WritePartners")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Partner partner)
    {
        if (id != partner.Id)
            return BadRequest();

        _context.Entry(partner).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PartnerExists(id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    private bool PartnerExists(int id)
        => _context.Partners.Any(e => e.Id == id);
}