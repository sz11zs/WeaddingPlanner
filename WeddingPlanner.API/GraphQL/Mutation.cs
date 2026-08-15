using HotChocolate;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Data;
using WeddingPlanner.Models;

namespace WeddingPlanner.API.GraphQL;

public class Mutation
{
    public async Task<Partner> AddPartner(
        string name,
        string? email,
        int categoryId,
        decimal commissionPct,
        [Service] IDbContextFactory<AppDbContext> dbContextFactory)
    {
        using var context =
            await dbContextFactory.CreateDbContextAsync();

        var partner = new Partner
        {
            Name = name,
            Email = email,
            CategoryId = categoryId,
            CommissionPct = commissionPct
        };

        context.Partners.Add(partner);
        await context.SaveChangesAsync();

        return partner;
    }

    public async Task<bool> DeletePartner(
        int id,
        [Service] IDbContextFactory<AppDbContext> dbContextFactory)
    {
        using var context =
            await dbContextFactory.CreateDbContextAsync();

        var partner = await context.Partners.FindAsync(id);

        if (partner == null)
            return false;

        context.Partners.Remove(partner);
        await context.SaveChangesAsync();

        return true;
    }
}
