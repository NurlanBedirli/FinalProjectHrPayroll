using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class fluentCascad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Educations_Employees_EmployeeId",
                table: "Educations");

            migrationBuilder.DropForeignKey(
                name: "FK_OldWorkPlaces_Employees_EmployeeId",
                table: "OldWorkPlaces");

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_Employees_EmployeeId",
                table: "Educations",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OldWorkPlaces_Employees_EmployeeId",
                table: "OldWorkPlaces",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Educations_Employees_EmployeeId",
                table: "Educations");

            migrationBuilder.DropForeignKey(
                name: "FK_OldWorkPlaces_Employees_EmployeeId",
                table: "OldWorkPlaces");

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_Employees_EmployeeId",
                table: "Educations",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OldWorkPlaces_Employees_EmployeeId",
                table: "OldWorkPlaces",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
