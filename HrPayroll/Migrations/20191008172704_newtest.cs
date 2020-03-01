using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class newtest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Placeswork_EmployeeId",
                table: "Placeswork");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaries_PositionsId",
                table: "EmployeeSalaries");

            migrationBuilder.CreateIndex(
                name: "IX_Placeswork_EmployeeId",
                table: "Placeswork",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_PositionsId",
                table: "EmployeeSalaries",
                column: "PositionsId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Placeswork_EmployeeId",
                table: "Placeswork");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaries_PositionsId",
                table: "EmployeeSalaries");

            migrationBuilder.CreateIndex(
                name: "IX_Placeswork_EmployeeId",
                table: "Placeswork",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaries_PositionsId",
                table: "EmployeeSalaries",
                column: "PositionsId");
        }
    }
}
