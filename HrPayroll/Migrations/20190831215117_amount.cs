using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class amount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PenaltyAmount",
                table: "SignInOutReasons",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PenaltyAmount",
                table: "SignInOutReasons");
        }
    }
}
