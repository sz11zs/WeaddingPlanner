using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<PartnerCategory> PartnerCategories => Set<PartnerCategory>();

    public DbSet<Partner> Partners => Set<Partner>();
}