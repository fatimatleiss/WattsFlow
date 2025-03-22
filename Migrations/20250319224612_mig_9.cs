using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WattsFlowProject.Migrations
{
    /// <inheritdoc />
    public partial class mig_9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrafficHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    PreviousTraffic = table.Column<double>(type: "double", nullable: false),
                    PreviousTrafficDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CurrentTraffic = table.Column<double>(type: "double", nullable: false),
                    CurrentTrafficDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Consumption = table.Column<double>(type: "double", nullable: false),
                    PriceUSD = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PriceLBP = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    RemainingAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrafficHistories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficHistories_CustomerId",
                table: "TrafficHistories",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrafficHistories");
        }
    }
}
