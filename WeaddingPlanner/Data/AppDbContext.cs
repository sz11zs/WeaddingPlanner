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

    public DbSet<Wedding> Weddings => Set<Wedding>();

    public DbSet<WeddingItem> WeddingItems => Set<WeddingItem>();

    public DbSet<Playlist> Playlists => Set<Playlist>();

    public DbSet<Arrangement> Arrangements => Set<Arrangement>();

    public DbSet<Menu> Menus => Set<Menu>();

    public DbSet<WeddingTemplate> WeddingTemplates => Set<WeddingTemplate>();

    public DbSet<WeddingTemplateItem> WeddingTemplateItems => Set<WeddingTemplateItem>();
}