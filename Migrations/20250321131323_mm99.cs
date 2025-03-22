using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WattsFlowProject.Migrations
{
    /// <inheritdoc />
    public partial class mm99 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Details_PostId",
                table: "Customers");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Details_PostId",
                table: "Customers",
                column: "PostId",
                principalTable: "Details",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Details_PostId",
                table: "Customers");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Details_PostId",
                table: "Customers",
                column: "PostId",
                principalTable: "Details",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
