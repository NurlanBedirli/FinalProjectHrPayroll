using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class plsiyer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDay",
                table: "Employees",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "PlasiyerCode",
                table: "Employees",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlasiyerCode",
                table: "Employees");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDay",
                table: "Employees",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
