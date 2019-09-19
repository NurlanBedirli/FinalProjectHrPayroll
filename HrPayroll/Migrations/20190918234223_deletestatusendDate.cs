using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class deletestatusendDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusEndDateTime",
                table: "WorkEnds");

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "WorkEnds",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salary",
                table: "WorkEnds");

            migrationBuilder.AddColumn<string>(
                name: "StatusEndDateTime",
                table: "WorkEnds",
                nullable: false,
                defaultValue: "");
        }
    }
}
