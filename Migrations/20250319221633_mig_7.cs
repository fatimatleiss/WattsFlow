using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WattsFlowProject.Migrations
{
    /// <inheritdoc />
    public partial class mig_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Traffics",
                newName: "PriceUSD");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaidAmount",
                table: "Traffics",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceLBP",
                table: "Traffics",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceLBP",
                table: "Traffics");

            migrationBuilder.RenameColumn(
                name: "PriceUSD",
                table: "Traffics",
                newName: "Price");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaidAmount",
                table: "Traffics",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");
        }
    }
}
