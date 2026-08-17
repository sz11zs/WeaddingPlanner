using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Data;
using WeddingPlanner.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace WeddingPlanner.Controllers;

public class WeddingsController : Controller
{
    private readonly AppDbContext _context;

    public WeddingsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Weddings
    public async Task<IActionResult> Index()
    {
        return View(await _context.Weddings.ToListAsync());
    }

    // GET: Weddings/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var wedding = await _context.Weddings
            .Include(w => w.Items)
                .ThenInclude(i => i.Partner)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (wedding == null)
            return NotFound();

        return View(wedding);
    }

    // GET: Weddings/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Weddings/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,Date,Location")] Wedding wedding)
    {
        if (ModelState.IsValid)
        {
            wedding.Date = DateTime.SpecifyKind(
                wedding.Date,
                DateTimeKind.Utc);

            _context.Add(wedding);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(wedding);
    }

    // GET: Weddings/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var wedding = await _context.Weddings.FindAsync(id);

        if (wedding == null)
            return NotFound();

        return View(wedding);
    }

    // POST: Weddings/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name,Date,Location")] Wedding wedding)
    {
        if (id != wedding.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                wedding.Date = DateTime.SpecifyKind(
                    wedding.Date,
                    DateTimeKind.Utc);

                _context.Update(wedding);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeddingExists(wedding.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(wedding);
    }

    // GET: Weddings/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var wedding = await _context.Weddings
            .FirstOrDefaultAsync(m => m.Id == id);

        if (wedding == null)
            return NotFound();

        return View(wedding);
    }

    // POST: Weddings/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var wedding = await _context.Weddings.FindAsync(id);

        if (wedding != null)
            _context.Weddings.Remove(wedding);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Pdf(int id)
    {
        var wedding = await _context.Weddings
            .Include(w => w.Items)
                .ThenInclude(i => i.Partner)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (wedding == null)
            return NotFound();

        var totalPrice = wedding.Items.Sum(i => i.Price);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);

                page.Header()
                    .Text("Izvješće o vjenčanju")
                    .FontSize(22)
                    .Bold();

                page.Content().Column(column =>
                {
                    column.Spacing(10);

                    column.Item().Text($"Naziv: {wedding.Name}");
                    column.Item().Text($"Datum: {wedding.Date:dd.MM.yyyy}");
                    column.Item().Text($"Lokacija: {wedding.Location}");

                    column.Item()
                        .PaddingTop(10)
                        .Text("Stavke vjenčanja")
                        .FontSize(16)
                        .Bold();

                    foreach (var item in wedding.Items)
                    {
                        var partnerName = item.Partner?.Name ?? "-";

                        column.Item().Text(
                            $"{item.Name} | {partnerName} | {item.Price:0.00} €");
                    }

                    column.Item()
                        .PaddingTop(10)
                        .Text($"Ukupna cijena: {totalPrice:0.00} €")
                        .FontSize(14)
                        .Bold();
                });
            });
        });

        var pdf = document.GeneratePdf();

        return File(
            pdf,
            "application/pdf",
            $"Vjencanje_{wedding.Id}.pdf");
    }
    private bool WeddingExists(int id)
    {
        return _context.Weddings.Any(e => e.Id == id);
    }
}