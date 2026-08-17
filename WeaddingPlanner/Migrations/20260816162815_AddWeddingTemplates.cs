using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WeaddingPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddWeddingTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeddingTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeddingTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeddingTemplateItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WeddingTemplateId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeddingTemplateItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeddingTemplateItems_WeddingTemplates_WeddingTemplateId",
                        column: x => x.WeddingTemplateId,
                        principalTable: "WeddingTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeddingTemplateItems_WeddingTemplateId",
                table: "WeddingTemplateItems",
                column: "WeddingTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeddingTemplateItems");

            migrationBuilder.DropTable(
                name: "WeddingTemplates");
        }
    }
}
