using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeaddingPlanner.Controllers;
using WeddingPlanner.Data;
using WeddingPlanner.Models;

namespace WeddingPlanner.Tests
{
    public class PartnersControllerTests
    {
        private AppDbContext KreirajBazu(string ime)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: ime)
                .Options;

            return new AppDbContext(options);
        }
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfPartners()
        {
            // Arrange — pripremi lažnu bazu s test podacima
            var context = KreirajBazu("TestBaza_Index");

            var kategorija = new PartnerCategory
            {
                Id = 1,
                Name = "Bend"
            };

            context.PartnerCategories.Add(kategorija);

            context.Partners.AddRange(
                new Partner
                {
                    Id = 1,
                    Name = "Partner 1",
                    CategoryId = 1
                },
                new Partner
                {
                    Id = 2,
                    Name = "Partner 2",
                    CategoryId = 1
                }
            );

            await context.SaveChangesAsync();

            var controller = new PartnersController(context);

            // Act — pozovi Index akciju
            var result = await controller.Index();

            // Assert — provjeri je li rezultat View s 2 partnera
            var viewResult = Assert.IsType<ViewResult>(result);

            var model =
                Assert.IsAssignableFrom<IEnumerable<Partner>>(viewResult.Model);

            Assert.Equal(2, model.Count());
        }
        [Fact]
        public async Task Create_ValidPartner_RedirectsToIndex()
        {
            // Arrange
            var context = KreirajBazu("TestBaza_Create");

            context.PartnerCategories.Add(
                new PartnerCategory
                {
                    Id = 1,
                    Name = "Bend"
                });

            await context.SaveChangesAsync();

            var controller = new PartnersController(context);

            var noviPartner = new Partner
            {
                Name = "Test Band",
                CategoryId = 1,
                Email = "test@test.com",
                CommissionPct = 10
            };

            // Act
            var result = await controller.Create(noviPartner);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal(1, context.Partners.Count());
        }
        [Fact]
        public async Task Delete_NonExistentId_ReturnsNotFound()
        {
            // Arrange — prazna baza, nema partnera
            var context = KreirajBazu("TestBaza_Delete");
            var controller = new PartnersController(context);

            // Act — pokušaj obrisati ID koji ne postoji
            var result = await controller.Delete(999);

            // Assert — mora vratiti NotFound (404)
            Assert.IsType<NotFoundResult>(result);
        }
    }

}
