using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class bonusAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Bonus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BonusStatus",
                table: "Bonus",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bonus_AppUserId",
                table: "Bonus",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bonus_AspNetUsers_AppUserId",
                table: "Bonus",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bonus_AspNetUsers_AppUserId",
                table: "Bonus");

            migrationBuilder.DropIndex(
                name: "IX_Bonus_AppUserId",
                table: "Bonus");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Bonus");

            migrationBuilder.DropColumn(
                name: "BonusStatus",
                table: "Bonus");
        }
    }
}
