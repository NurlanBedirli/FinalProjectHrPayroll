using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class appusermenecer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmporiumAppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmporiumId = table.Column<int>(nullable: false),
                    AppUserId1 = table.Column<string>(nullable: true),
                    AppUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmporiumAppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmporiumAppUsers_AspNetUsers_AppUserId1",
                        column: x => x.AppUserId1,
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
                name: "IX_EmporiumAppUsers_AppUserId1",
                table: "EmporiumAppUsers",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_EmporiumAppUsers_EmporiumId",
                table: "EmporiumAppUsers",
                column: "EmporiumId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmporiumAppUsers");
        }
    }
}
