using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class disciplness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "DisciplinePenalties");

            migrationBuilder.AddColumn<int>(
                name: "MaxDay",
                table: "DisciplinePenalties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinDay",
                table: "DisciplinePenalties",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxDay",
                table: "DisciplinePenalties");

            migrationBuilder.DropColumn(
                name: "MinDay",
                table: "DisciplinePenalties");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DisciplinePenalties",
                nullable: true);
        }
    }
}
