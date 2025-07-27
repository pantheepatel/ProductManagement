using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class upgradeModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ProductPrices",
                newName: "SeasonalPrice");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "Invoices",
                newName: "PriceTotal");

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalItems",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TotalItems",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "SeasonalPrice",
                table: "ProductPrices",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "PriceTotal",
                table: "Invoices",
                newName: "SubTotal");
        }
    }
}
