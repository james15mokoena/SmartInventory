using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartInventory.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedRequisitionItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequisitionItem",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SellingPrice = table.Column<double>(type: "double", nullable: false),
                    RequisitionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionItem", x => x.Code);
                    table.ForeignKey(
                        name: "FK_RequisitionItem_Requisition_RequisitionId",
                        column: x => x.RequisitionId,
                        principalTable: "Requisition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionItem_RequisitionId",
                table: "RequisitionItem",
                column: "RequisitionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequisitionItem");
        }
    }
}
