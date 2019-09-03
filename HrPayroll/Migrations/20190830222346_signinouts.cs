using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class signinouts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignOutReasons");

            migrationBuilder.AddColumn<decimal>(
                name: "PenaltyAmount",
                table: "SignInReasons",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "RaasonName",
                table: "SignInReasons",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SignInReasons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PenaltyAmount",
                table: "SignInReasons");

            migrationBuilder.DropColumn(
                name: "RaasonName",
                table: "SignInReasons");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SignInReasons");

            migrationBuilder.CreateTable(
                name: "SignOutReasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(nullable: false),
                    PenaltyAmount = table.Column<decimal>(nullable: false),
                    RaasonName = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    signOut = table.Column<bool>(nullable: false),
                    signOutDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignOutReasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignOutReasons_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SignOutReasons_EmployeeId",
                table: "SignOutReasons",
                column: "EmployeeId");
        }
    }
}
