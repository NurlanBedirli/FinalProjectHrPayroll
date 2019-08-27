using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class appuserIdint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmporiumAppUsers_Emporia_EmporiumId1",
                table: "EmporiumAppUsers");

            migrationBuilder.DropIndex(
                name: "IX_EmporiumAppUsers_EmporiumId1",
                table: "EmporiumAppUsers");

            migrationBuilder.DropColumn(
                name: "EmporiumId1",
                table: "EmporiumAppUsers");

            migrationBuilder.AlterColumn<int>(
                name: "EmporiumId",
                table: "EmporiumAppUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmporiumAppUsers_EmporiumId",
                table: "EmporiumAppUsers",
                column: "EmporiumId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmporiumAppUsers_Emporia_EmporiumId",
                table: "EmporiumAppUsers",
                column: "EmporiumId",
                principalTable: "Emporia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmporiumAppUsers_Emporia_EmporiumId",
                table: "EmporiumAppUsers");

            migrationBuilder.DropIndex(
                name: "IX_EmporiumAppUsers_EmporiumId",
                table: "EmporiumAppUsers");

            migrationBuilder.AlterColumn<string>(
                name: "EmporiumId",
                table: "EmporiumAppUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "EmporiumId1",
                table: "EmporiumAppUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmporiumAppUsers_EmporiumId1",
                table: "EmporiumAppUsers",
                column: "EmporiumId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EmporiumAppUsers_Emporia_EmporiumId1",
                table: "EmporiumAppUsers",
                column: "EmporiumId1",
                principalTable: "Emporia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
