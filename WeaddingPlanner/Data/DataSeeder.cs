using Bogus;
using WeddingPlanner.Models;

namespace WeddingPlanner.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context.PartnerCategories.Any())
            return;

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
}