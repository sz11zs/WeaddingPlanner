using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeaddingPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddPartnerToWeddingItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartnerId",
                table: "WeddingItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeddingItems_PartnerId",
                table: "WeddingItems",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeddingItems_Partners_PartnerId",
                table: "WeddingItems",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeddingItems_Partners_PartnerId",
                table: "WeddingItems");

            migrationBuilder.DropIndex(
                name: "IX_WeddingItems_PartnerId",
                table: "WeddingItems");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "WeddingItems");
        }
    }
}
