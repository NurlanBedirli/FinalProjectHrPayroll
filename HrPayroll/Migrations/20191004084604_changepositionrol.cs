using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class changepositionrol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OficeEmployees_AspNetUsers_AppUserId1",
                table: "OficeEmployees");

            migrationBuilder.DropIndex(
                name: "IX_OficeEmployees_AppUserId1",
                table: "OficeEmployees");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "OficeEmployees");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "OficeEmployees",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "ChangePositionRols",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CalcSalary = table.Column<bool>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Company = table.Column<string>(nullable: true),
                    Emporium = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangePositionRols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangePositionRols_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OficeEmployees_AppUserId",
                table: "OficeEmployees",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangePositionRols_AppUserId",
                table: "ChangePositionRols",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OficeEmployees_AspNetUsers_AppUserId",
                table: "OficeEmployees",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OficeEmployees_AspNetUsers_AppUserId",
                table: "OficeEmployees");

            migrationBuilder.DropTable(
                name: "ChangePositionRols");

            migrationBuilder.DropIndex(
                name: "IX_OficeEmployees_AppUserId",
                table: "OficeEmployees");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "OficeEmployees",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

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
    }
}
