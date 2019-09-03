using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class reasonsigninouts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignOutReasonTbl_Employees_EmployeeId",
                table: "SignOutReasonTbl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SignOutReasonTbl",
                table: "SignOutReasonTbl");

            migrationBuilder.RenameTable(
                name: "SignOutReasonTbl",
                newName: "SignOutReasons");

            migrationBuilder.RenameIndex(
                name: "IX_SignOutReasonTbl_EmployeeId",
                table: "SignOutReasons",
                newName: "IX_SignOutReasons_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SignOutReasons",
                table: "SignOutReasons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SignOutReasons_Employees_EmployeeId",
                table: "SignOutReasons",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignOutReasons_Employees_EmployeeId",
                table: "SignOutReasons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SignOutReasons",
                table: "SignOutReasons");

            migrationBuilder.RenameTable(
                name: "SignOutReasons",
                newName: "SignOutReasonTbl");

            migrationBuilder.RenameIndex(
                name: "IX_SignOutReasons_EmployeeId",
                table: "SignOutReasonTbl",
                newName: "IX_SignOutReasonTbl_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SignOutReasonTbl",
                table: "SignOutReasonTbl",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SignOutReasonTbl_Employees_EmployeeId",
                table: "SignOutReasonTbl",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
