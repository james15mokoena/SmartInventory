using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartInventory.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedMaximumStockLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaximumStockLevel",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaximumStockLevel",
                table: "Product");
        }
    }
}
