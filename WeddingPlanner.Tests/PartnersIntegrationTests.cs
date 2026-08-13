using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeddingPlanner.Data;
using WeddingPlanner.Models;

namespace WeddingPlanner.Tests
{
    public class PartnersIntegrationTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public PartnersIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");

                builder.ConfigureServices(services =>
                {
                    var toRemove = services.Where(d =>
                        d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                        d.ServiceType == typeof(DbContext) ||
                        d.ServiceType == typeof(AppDbContext) ||
                        d.ServiceType.Name.Contains("DbContext"))
                        .ToList();

                    foreach (var d in toRemove)
                        services.Remove(d);

                    var dbName = "IntTestBaza_" + Guid.NewGuid();

                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase(dbName));

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();

                    var ctx = scope.ServiceProvider
                        .GetRequiredService<AppDbContext>();

                    ctx.Database.EnsureCreated();

                    ctx.PartnerCategories.Add(
                        new PartnerCategory
                        {
                            Id = 1,
                            Name = "Bend"
                        });

                    ctx.Partners.Add(
                        new Partner
                        {
                            Id = 1,
                            Name = "Test",
                            CategoryId = 1,
                            Email = "t@t.com",
                            CommissionPct = 10
                        });

                    ctx.SaveChanges();
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_Partners_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/Partners");

            response.EnsureSuccessStatusCode();
        }
    }
}