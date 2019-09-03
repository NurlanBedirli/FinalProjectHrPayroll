using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class signinoutss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignInReasons_Employees_EmployeeId",
                table: "SignInReasons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SignInReasons",
                table: "SignInReasons");

            migrationBuilder.RenameTable(
                name: "SignInReasons",
                newName: "SignInOutReasons");

            migrationBuilder.RenameIndex(
                name: "IX_SignInReasons_EmployeeId",
                table: "SignInOutReasons",
                newName: "IX_SignInOutReasons_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SignInOutReasons",
                table: "SignInOutReasons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SignInOutReasons_Employees_EmployeeId",
                table: "SignInOutReasons",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignInOutReasons_Employees_EmployeeId",
                table: "SignInOutReasons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SignInOutReasons",
                table: "SignInOutReasons");

            migrationBuilder.RenameTable(
                name: "SignInOutReasons",
                newName: "SignInReasons");

            migrationBuilder.RenameIndex(
                name: "IX_SignInOutReasons_EmployeeId",
                table: "SignInReasons",
                newName: "IX_SignInReasons_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SignInReasons",
                table: "SignInReasons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SignInReasons_Employees_EmployeeId",
                table: "SignInReasons",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
