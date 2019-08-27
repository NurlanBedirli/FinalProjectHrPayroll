using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class addworkolds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OldWorkPlace_Employees_EmployeeId",
                table: "OldWorkPlace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OldWorkPlace",
                table: "OldWorkPlace");

            migrationBuilder.RenameTable(
                name: "OldWorkPlace",
                newName: "OldWorkPlaces");

            migrationBuilder.RenameIndex(
                name: "IX_OldWorkPlace_EmployeeId",
                table: "OldWorkPlaces",
                newName: "IX_OldWorkPlaces_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OldWorkPlaces",
                table: "OldWorkPlaces",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OldWorkPlaces_Employees_EmployeeId",
                table: "OldWorkPlaces",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OldWorkPlaces_Employees_EmployeeId",
                table: "OldWorkPlaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OldWorkPlaces",
                table: "OldWorkPlaces");

            migrationBuilder.RenameTable(
                name: "OldWorkPlaces",
                newName: "OldWorkPlace");

            migrationBuilder.RenameIndex(
                name: "IX_OldWorkPlaces_EmployeeId",
                table: "OldWorkPlace",
                newName: "IX_OldWorkPlace_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OldWorkPlace",
                table: "OldWorkPlace",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OldWorkPlace_Employees_EmployeeId",
                table: "OldWorkPlace",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
