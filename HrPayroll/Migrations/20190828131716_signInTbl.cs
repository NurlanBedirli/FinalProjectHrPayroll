using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class signInTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PenaltyDate",
                table: "WorkReasonStatuses",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddColumn<bool>(
                name: "signIn",
                table: "WorkReasonStatuses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "signIn",
                table: "WorkReasonStatuses");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PenaltyDate",
                table: "WorkReasonStatuses",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
