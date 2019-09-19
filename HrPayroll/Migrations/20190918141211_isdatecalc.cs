using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class isdatecalc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCalcDate",
                table: "WorkEnds",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "BonusStatus",
                table: "Bonus",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCalcDate",
                table: "WorkEnds");

            migrationBuilder.AlterColumn<string>(
                name: "BonusStatus",
                table: "Bonus",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
