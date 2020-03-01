using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class monthsale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmporiumMonths",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Prize = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    EmporiumId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmporiumMonths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmporiumMonths_Emporia_EmporiumId",
                        column: x => x.EmporiumId,
                        principalTable: "Emporia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthSales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Prize = table.Column<decimal>(nullable: false),
                    EmporiumId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthSales_Emporia_EmporiumId",
                        column: x => x.EmporiumId,
                        principalTable: "Emporia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmporiumMonths_EmporiumId",
                table: "EmporiumMonths",
                column: "EmporiumId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthSales_EmporiumId",
                table: "MonthSales",
                column: "EmporiumId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmporiumMonths");

            migrationBuilder.DropTable(
                name: "MonthSales");
        }
    }
}
