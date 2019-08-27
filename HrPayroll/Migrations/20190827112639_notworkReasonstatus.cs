using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrPayroll.Migrations
{
    public partial class notworkReasonstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "NotWorkReasons");

            migrationBuilder.CreateTable(
                name: "WorkReasonStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(nullable: true),
                    EmployeeNotWorkReasonId = table.Column<int>(nullable: false)
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
                name: "IX_WorkReasonStatuses_EmployeeNotWorkReasonId",
                table: "WorkReasonStatuses",
                column: "EmployeeNotWorkReasonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkReasonStatuses");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "NotWorkReasons",
                nullable: true);
        }
    }
}
