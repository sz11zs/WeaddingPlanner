using Bogus;
using WeddingPlanner.Models;

namespace WeddingPlanner.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!context.PartnerCategories.Any())
        {
            var categories = new List<PartnerCategory>
            {
                new() { Name = "Bend/DJ", Description = "Glazba" },
                new() { Name = "Cvjećar", Description = "Cvijeće" },
                new() { Name = "Restoran/Dvorana", Description = "Prostor i hrana" },
                new() { Name = "Slastičarnica", Description = "Torte i kolači" }
            };

            context.PartnerCategories.AddRange(categories);
            await context.SaveChangesAsync();

            var partnerFaker = new Faker<Partner>("hr")
                .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).Id)
                .RuleFor(p => p.Name, f => f.Company.CompanyName())
                .RuleFor(p => p.Address, f => f.Address.FullAddress())
                .RuleFor(p => p.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(p => p.Email, f => f.Internet.Email())
                .RuleFor(p => p.CommissionPct, f => f.Random.Decimal(5, 15));

            context.Partners.AddRange(partnerFaker.Generate(20));
            await context.SaveChangesAsync();
        }

        if (!context.Weddings.Any())
        {
            var weddingFaker = new Faker<Wedding>("hr")
                .RuleFor(w => w.Name,
                    f => $"Vjenčanje {f.Name.FirstName()} i {f.Name.FirstName()}")
                .RuleFor(w => w.Date,
                    f => f.Date.Future(2).ToUniversalTime())
                .RuleFor(w => w.Location,
                    f => f.Address.City());

            var weddings = weddingFaker.Generate(5);

            context.Weddings.AddRange(weddings);
            await context.SaveChangesAsync();

            var weddingItems = new List<WeddingItem>();

            foreach (var wedding in weddings)
            {
                var itemFaker = new Faker<WeddingItem>("hr")
                    .RuleFor(i => i.WeddingId, wedding.Id)
                    .RuleFor(i => i.Name, f => f.PickRandom(
                        "Glazba",
                        "Cvijeće",
                        "Fotograf",
                        "Torta",
                        "Dekoracija",
                        "Sala"))
                    .RuleFor(i => i.Price, f =>
                        Math.Round(f.Random.Decimal(100, 3000), 2));

                weddingItems.AddRange(itemFaker.Generate(4));
            }

            context.WeddingItems.AddRange(weddingItems);
            await context.SaveChangesAsync();
        }

        if (!context.Playlists.Any())
        {
            var bandCategory = context.PartnerCategories
                .FirstOrDefault(c => c.Name == "Bend/DJ");

            if (bandCategory != null)
            {
                var bands = context.Partners
                    .Where(p => p.CategoryId == bandCategory.Id)
                    .ToList();

                var playlists = new List<Playlist>();

                foreach (var band in bands)
                {
                    var playlistFaker = new Faker<Playlist>("hr")
                        .RuleFor(p => p.PartnerId, band.Id)
                        .RuleFor(p => p.Name, f => f.PickRandom(
                            "Pop hitovi",
                            "Rock klasici",
                            "Domaća glazba",
                            "80s & 90s",
                            "Party mix"))
                        .RuleFor(p => p.Price,
                            f => Math.Round(f.Random.Decimal(500, 2000), 2));

                    playlists.AddRange(playlistFaker.Generate(2));
                }

                context.Playlists.AddRange(playlists);
                await context.SaveChangesAsync();
            }
        }

        if (!context.Arrangements.Any())
        {
            var floristCategory = context.PartnerCategories
                .FirstOrDefault(c => c.Name == "Cvjećar");

            if (floristCategory != null)
            {
                var florists = context.Partners
                    .Where(p => p.CategoryId == floristCategory.Id)
                    .ToList();

                var arrangements = new List<Arrangement>();

                foreach (var florist in florists)
                {
                    var arrangementFaker = new Faker<Arrangement>("hr")
                        .RuleFor(a => a.PartnerId, florist.Id)
                        .RuleFor(a => a.Name, f => f.PickRandom(
                            "Buket mladenke",
                            "Dekoracija stolova",
                            "Cvjetni luk",
                            "Dekoracija dvorane",
                            "Reveri"))
                        .RuleFor(a => a.Price,
                            f => Math.Round(f.Random.Decimal(50, 1000), 2));

                    arrangements.AddRange(arrangementFaker.Generate(2));
                }

                context.Arrangements.AddRange(arrangements);
                await context.SaveChangesAsync();
            }
        }

        if (!context.Menus.Any())
        {
            var restaurantCategory = context.PartnerCategories
                .FirstOrDefault(c => c.Name == "Restoran/Dvorana");

            if (restaurantCategory != null)
            {
                var restaurants = context.Partners
                    .Where(p => p.CategoryId == restaurantCategory.Id)
                    .ToList();

                var menus = new List<Menu>();

                foreach (var restaurant in restaurants)
                {
                    var menuFaker = new Faker<Menu>("hr")
                        .RuleFor(m => m.PartnerId, restaurant.Id)
                        .RuleFor(m => m.Name, f => f.PickRandom(
                            "Klasični meni",
                            "Svečani meni",
                            "Vegetarijanski meni",
                            "Premium meni",
                            "Tradicionalni meni"))
                        .RuleFor(m => m.Price,
                            f => Math.Round(f.Random.Decimal(30, 150), 2));

                    menus.AddRange(menuFaker.Generate(2));
                }

                context.Menus.AddRange(menus);
                await context.SaveChangesAsync();
            }
        }

        if (!context.WeddingTemplates.Any())
        {
            var templateFaker = new Faker<WeddingTemplate>("hr")
                .RuleFor(t => t.Name, f => f.PickRandom(
                    "Klasično vjenčanje",
                    "Elegantno vjenčanje",
                    "Malo vjenčanje"));

            var templates = templateFaker.Generate(3);

            context.WeddingTemplates.AddRange(templates);
            await context.SaveChangesAsync();

            var templateItems = new List<WeddingTemplateItem>();

            var itemNames = new[]
            {
                "Glazba",
                "Cvijeće",
                "Dvorana",
                "Hrana",
                "Torta",
                "Fotograf"
            };

            foreach (var template in templates)
            {
                foreach (var itemName in itemNames)
                {
                    templateItems.Add(new WeddingTemplateItem
                    {
                        WeddingTemplateId = template.Id,
                        Name = itemName
                    });
                }
            }

            context.WeddingTemplateItems.AddRange(templateItems);
            await context.SaveChangesAsync();
        }

        // Poveži postojeće WeddingItems s partnerima prema vrsti stavke
        if (context.WeddingItems.Any(i => i.PartnerId == null))
        {
            var band = context.Partners
                .FirstOrDefault(p => p.Category.Name == "Bend/DJ");

            var florist = context.Partners
                .FirstOrDefault(p => p.Category.Name == "Cvjećar");

            var restaurant = context.Partners
                .FirstOrDefault(p => p.Category.Name == "Restoran/Dvorana");

            var cakeShop = context.Partners
                .FirstOrDefault(p => p.Category.Name == "Slastičarnica");

            var weddingItems = context.WeddingItems
                .Where(i => i.PartnerId == null)
                .ToList();

            foreach (var item in weddingItems)
            {
                if (item.Name == "Glazba" && band != null)
                    item.PartnerId = band.Id;

                else if ((item.Name == "Cvijeće" || item.Name == "Dekoracija")
                         && florist != null)
                    item.PartnerId = florist.Id;

                else if (item.Name == "Sala" && restaurant != null)
                    item.PartnerId = restaurant.Id;

                else if (item.Name == "Torta" && cakeShop != null)
                    item.PartnerId = cakeShop.Id;
            }

            await context.SaveChangesAsync();
        }
    }
}