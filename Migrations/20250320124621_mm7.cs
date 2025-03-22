using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WattsFlowProject.Migrations
{
    /// <inheritdoc />
    public partial class mm7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Consumption",
                table: "Traffics",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingAmount",
                table: "Traffics",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Consumption",
                table: "Traffics");

            migrationBuilder.DropColumn(
                name: "RemainingAmount",
                table: "Traffics");
        }
    }
}
