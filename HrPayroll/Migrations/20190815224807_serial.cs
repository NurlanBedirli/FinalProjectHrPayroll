using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class serial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IDCardNumber",
                table: "Employees",
                newName: "Nationally");

            migrationBuilder.RenameColumn(
                name: "Districtegistration",
                table: "Employees",
                newName: "IDCardSerialNumber");

            migrationBuilder.AddColumn<string>(
                name: "DistrictRegistration",
                table: "Employees",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictRegistration",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "Nationally",
                table: "Employees",
                newName: "IDCardNumber");

            migrationBuilder.RenameColumn(
                name: "IDCardSerialNumber",
                table: "Employees",
                newName: "Districtegistration");
        }
    }
}
