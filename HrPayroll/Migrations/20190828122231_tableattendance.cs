using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class tableattendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceTables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttendanceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SignInTbls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SignIn = table.Column<bool>(nullable: false),
                    SignInTime = table.Column<DateTime>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    AttendanceTableId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignInTbls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignInTbls_AttendanceTables_AttendanceTableId",
                        column: x => x.AttendanceTableId,
                        principalTable: "AttendanceTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SignInTbls_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SignOutTbls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SignOutTime = table.Column<DateTime>(nullable: false),
                    SignOut = table.Column<bool>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    AttendanceTableId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignOutTbls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignOutTbls_AttendanceTables_AttendanceTableId",
                        column: x => x.AttendanceTableId,
                        principalTable: "AttendanceTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SignOutTbls_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SignInTbls_AttendanceTableId",
                table: "SignInTbls",
                column: "AttendanceTableId");

            migrationBuilder.CreateIndex(
                name: "IX_SignInTbls_EmployeeId",
                table: "SignInTbls",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SignOutTbls_AttendanceTableId",
                table: "SignOutTbls",
                column: "AttendanceTableId");

            migrationBuilder.CreateIndex(
                name: "IX_SignOutTbls_EmployeeId",
                table: "SignOutTbls",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignInTbls");

            migrationBuilder.DropTable(
                name: "SignOutTbls");

            migrationBuilder.DropTable(
                name: "AttendanceTables");
        }
    }
}
