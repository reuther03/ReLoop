using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReLoop.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdatesItemEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Items_Category",
                table: "Items",
                column: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Items_Category",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Items");
        }
    }
}
