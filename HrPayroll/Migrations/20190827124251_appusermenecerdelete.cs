using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class appusermenecerdelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmporiumAppUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmporiumAppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<string>(nullable: true),
                    EmporiumId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmporiumAppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmporiumAppUsers_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmporiumAppUsers_Emporia_EmporiumId",
                        column: x => x.EmporiumId,
                        principalTable: "Emporia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmporiumAppUsers_AppUserId",
                table: "EmporiumAppUsers",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmporiumAppUsers_EmporiumId",
                table: "EmporiumAppUsers",
                column: "EmporiumId");
        }
    }
}
