using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class signoutchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "signIn",
                table: "WorkReasonStatuses",
                newName: "signOut");

            migrationBuilder.RenameColumn(
                name: "PenaltyDate",
                table: "WorkReasonStatuses",
                newName: "signOutDate");

            migrationBuilder.CreateTable(
                name: "SignInTbls",
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
                    table.PrimaryKey("PK_SignInTbls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignInTbls_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SignInTbls_EmployeeId",
                table: "SignInTbls",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignInTbls");

            migrationBuilder.RenameColumn(
                name: "signOutDate",
                table: "WorkReasonStatuses",
                newName: "PenaltyDate");

            migrationBuilder.RenameColumn(
                name: "signOut",
                table: "WorkReasonStatuses",
                newName: "signIn");
        }
    }
}
