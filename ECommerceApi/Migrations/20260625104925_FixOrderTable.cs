using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceApi.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Orders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "productionId",
                table: "Orders",
                newName: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Orders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Orders",
                newName: "productionId");
        }
    }
}
