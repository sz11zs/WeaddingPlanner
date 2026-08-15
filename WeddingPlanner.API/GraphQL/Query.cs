using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Data;
using WeddingPlanner.Models;

namespace WeddingPlanner.API.GraphQL;

public class Query
{
    [UseFiltering]
    [UseSorting]
    public IQueryable<Partner> GetPartners(
        [Service] IDbContextFactory<AppDbContext> dbContextFactory)
    {
        var context = dbContextFactory.CreateDbContext();

        return context.Partners
            .Include(p => p.Category);
    }

    public async Task<Partner?> GetPartner(
        int id,
        [Service] IDbContextFactory<AppDbContext> dbContextFactory)
    {
        using var context =
            await dbContextFactory.CreateDbContextAsync();

        return await context.Partners
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}