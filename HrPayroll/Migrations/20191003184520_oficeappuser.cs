using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class oficeappuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "OficeEmployees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "OficeEmployees",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OficeEmployees_AppUserId1",
                table: "OficeEmployees",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OficeEmployees_AspNetUsers_AppUserId1",
                table: "OficeEmployees",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OficeEmployees_AspNetUsers_AppUserId1",
                table: "OficeEmployees");

            migrationBuilder.DropIndex(
                name: "IX_OficeEmployees_AppUserId1",
                table: "OficeEmployees");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "OficeEmployees");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "OficeEmployees");
        }
    }
}
