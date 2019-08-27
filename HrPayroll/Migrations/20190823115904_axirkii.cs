using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class axirkii : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PositionsDepartaments_Positions_PositionsId",
                table: "PositionsDepartaments");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "PositionsDepartaments");

            migrationBuilder.AlterColumn<int>(
                name: "PositionsId",
                table: "PositionsDepartaments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PositionsDepartaments_Positions_PositionsId",
                table: "PositionsDepartaments",
                column: "PositionsId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PositionsDepartaments_Positions_PositionsId",
                table: "PositionsDepartaments");

            migrationBuilder.AlterColumn<int>(
                name: "PositionsId",
                table: "PositionsDepartaments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "PositionsDepartaments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PositionsDepartaments_Positions_PositionsId",
                table: "PositionsDepartaments",
                column: "PositionsId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
