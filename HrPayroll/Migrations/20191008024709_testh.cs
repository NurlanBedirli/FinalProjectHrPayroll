using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class testh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Departaments_HoldingId",
                table: "Departaments");

            migrationBuilder.CreateIndex(
                name: "IX_Departaments_HoldingId",
                table: "Departaments",
                column: "HoldingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Departaments_HoldingId",
                table: "Departaments");

            migrationBuilder.CreateIndex(
                name: "IX_Departaments_HoldingId",
                table: "Departaments",
                column: "HoldingId",
                unique: true);
        }
    }
}
