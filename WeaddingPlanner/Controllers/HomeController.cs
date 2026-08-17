using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WeaddingPlanner.Models;
using WeddingPlanner.Data;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Home
    public async Task<IActionResult> Index()
    {
        await LoadDropdowns();

        return View();
    }

    // POST: Home
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        string name,
        DateTime date,
        int? restaurantId,
        int? menuId,
        int? bandId,
        int? playlistId,
        int? floristId,
        int? arrangementId,
        int? cakeShopId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            await LoadDropdowns();

            return View();
        }

        var restaurant = restaurantId.HasValue
            ? await _context.Partners.FindAsync(restaurantId.Value)
            : null;

        var wedding = new Wedding
        {
            Name = name,
            Date = DateTime.SpecifyKind(date, DateTimeKind.Utc),

            // Ako je odabrana dvorana/restoran,
            // koristimo njegovu adresu kao lokaciju
            Location = restaurant?.Address
        };

        _context.Weddings.Add(wedding);
        await _context.SaveChangesAsync();

        var weddingItems = new List<WeddingItem>();

        // RESTORAN / DVORANA
        if (restaurantId.HasValue)
        {
            weddingItems.Add(new WeddingItem
            {
                WeddingId = wedding.Id,
                Name = "Dvorana",
                PartnerId = restaurantId.Value,
                Price = 0
            });
        }

        // MENI
        if (menuId.HasValue)
        {
            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.Id == menuId.Value);

            if (menu != null)
            {
                weddingItems.Add(new WeddingItem
                {
                    WeddingId = wedding.Id,
                    Name = $"Hrana - {menu.Name}",
                    PartnerId = menu.PartnerId,
                    Price = menu.Price
                });
            }
        }

        // BEND / DJ
        if (bandId.HasValue)
        {
            weddingItems.Add(new WeddingItem
            {
                WeddingId = wedding.Id,
                Name = "Glazba",
                PartnerId = bandId.Value,
                Price = 0
            });
        }

        // PLAYLISTA
        if (playlistId.HasValue)
        {
            var playlist = await _context.Playlists
                .FirstOrDefaultAsync(p => p.Id == playlistId.Value);

            if (playlist != null)
            {
                weddingItems.Add(new WeddingItem
                {
                    WeddingId = wedding.Id,
                    Name = $"Playlista - {playlist.Name}",
                    PartnerId = playlist.PartnerId,
                    Price = playlist.Price
                });
            }
        }

        // CVJEĆAR
        if (floristId.HasValue)
        {
            weddingItems.Add(new WeddingItem
            {
                WeddingId = wedding.Id,
                Name = "Cvijeće",
                PartnerId = floristId.Value,
                Price = 0
            });
        }

        // ARANŽMAN
        if (arrangementId.HasValue)
        {
            var arrangement = await _context.Arrangements
                .FirstOrDefaultAsync(a => a.Id == arrangementId.Value);

            if (arrangement != null)
            {
                weddingItems.Add(new WeddingItem
                {
                    WeddingId = wedding.Id,
                    Name = $"Aranžman - {arrangement.Name}",
                    PartnerId = arrangement.PartnerId,
                    Price = arrangement.Price
                });
            }
        }

        // SLASTIČARNICA
        if (cakeShopId.HasValue)
        {
            weddingItems.Add(new WeddingItem
            {
                WeddingId = wedding.Id,
                Name = "Torta",
                PartnerId = cakeShopId.Value,
                Price = 0
            });
        }

        _context.WeddingItems.AddRange(weddingItems);
        await _context.SaveChangesAsync();

        return RedirectToAction(
            "Details",
            "Weddings",
            new { id = wedding.Id });
    }

    private async Task LoadDropdowns()
    {
        var restaurants = await _context.Partners
            .Where(p => p.Category.Name == "Restoran/Dvorana")
            .ToListAsync();

        var bands = await _context.Partners
            .Where(p => p.Category.Name == "Bend/DJ")
            .ToListAsync();

        var florists = await _context.Partners
            .Where(p => p.Category.Name == "Cvjećar")
            .ToListAsync();

        var cakeShops = await _context.Partners
            .Where(p => p.Category.Name == "Slastičarnica")
            .ToListAsync();

        ViewBag.Restaurants =
            new SelectList(restaurants, "Id", "Name");

        ViewBag.Bands =
            new SelectList(bands, "Id", "Name");

        ViewBag.Florists =
            new SelectList(florists, "Id", "Name");

        ViewBag.CakeShops =
            new SelectList(cakeShops, "Id", "Name");

        ViewBag.Menus =
            new SelectList(
                await _context.Menus.ToListAsync(),
                "Id",
                "Name");

        ViewBag.Playlists =
            new SelectList(
                await _context.Playlists.ToListAsync(),
                "Id",
                "Name");

        ViewBag.Arrangements =
            new SelectList(
                await _context.Arrangements.ToListAsync(),
                "Id",
                "Name");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId =
                Activity.Current?.Id ??
                HttpContext.TraceIdentifier
        });
    }
}