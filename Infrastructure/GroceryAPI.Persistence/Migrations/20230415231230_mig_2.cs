using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroceryAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Products",
                newName: "Barcode");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Barcode",
                table: "Products",
                newName: "Image");
        }
    }
}
