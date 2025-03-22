using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WattsFlowProject.Migrations
{
    /// <inheritdoc />
    public partial class mig_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Traffics",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "SystemSettings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Traffics");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "SystemSettings");
        }
    }
}
