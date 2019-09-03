using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class reasonsigninout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeAttandances");

            migrationBuilder.DropTable(
                name: "SignInTbls");

            migrationBuilder.DropTable(
                name: "WorkReasonStatuses");

            migrationBuilder.DropTable(
                name: "NotWorkReasons");

            migrationBuilder.CreateTable(
                name: "SignInReasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SignInTime = table.Column<DateTime>(nullable: false),
                    SignIn = table.Column<bool>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignInReasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignInReasons_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SignOutReasonTbl",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RaasonName = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    PenaltyAmount = table.Column<decimal>(nullable: false),
                    signOutDate = table.Column<DateTime>(nullable: false),
                    signOut = table.Column<bool>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignOutReasonTbl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignOutReasonTbl_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SignInReasons_EmployeeId",
                table: "SignInReasons",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SignOutReasonTbl_EmployeeId",
                table: "SignOutReasonTbl",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignInReasons");

            migrationBuilder.DropTable(
                name: "SignOutReasonTbl");

            migrationBuilder.CreateTable(
                name: "NotWorkReasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotWorkReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SignInTbls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(nullable: false),
                    SignIn = table.Column<bool>(nullable: false),
                    SignInTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignInTbls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignInTbls_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAttandances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(nullable: false),
                    NotWorkReasonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAttandances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAttandances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeAttandances_NotWorkReasons_NotWorkReasonId",
                        column: x => x.NotWorkReasonId,
                        principalTable: "NotWorkReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkReasonStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeNotWorkReasonId = table.Column<int>(nullable: false),
                    PenaltyAmount = table.Column<decimal>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    signOut = table.Column<bool>(nullable: false),
                    signOutDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkReasonStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkReasonStatuses_NotWorkReasons_EmployeeNotWorkReasonId",
                        column: x => x.EmployeeNotWorkReasonId,
                        principalTable: "NotWorkReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttandances_EmployeeId",
                table: "EmployeeAttandances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttandances_NotWorkReasonId",
                table: "EmployeeAttandances",
                column: "NotWorkReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_SignInTbls_EmployeeId",
                table: "SignInTbls",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkReasonStatuses_EmployeeNotWorkReasonId",
                table: "WorkReasonStatuses",
                column: "EmployeeNotWorkReasonId");
        }
    }
}
