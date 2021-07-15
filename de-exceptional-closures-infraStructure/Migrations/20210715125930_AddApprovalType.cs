using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace de_exceptional_closures_Infrastructure.Migrations
{
    public partial class AddApprovalType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApprovalTypeId",
                table: "ClosureReason",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ApprovalType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ApprovalType",
                columns: new[] { "Id", "Description" },
                values: new object[] { 1, "Pre-approved" });

            migrationBuilder.InsertData(
                table: "ApprovalType",
                columns: new[] { "Id", "Description" },
                values: new object[] { 2, "Approval required" });

            migrationBuilder.CreateIndex(
                name: "IX_ClosureReason_ApprovalTypeId",
                table: "ClosureReason",
                column: "ApprovalTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClosureReason_ApprovalType_ApprovalTypeId",
                table: "ClosureReason",
                column: "ApprovalTypeId",
                principalTable: "ApprovalType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClosureReason_ApprovalType_ApprovalTypeId",
                table: "ClosureReason");

            migrationBuilder.DropTable(
                name: "ApprovalType");

            migrationBuilder.DropIndex(
                name: "IX_ClosureReason_ApprovalTypeId",
                table: "ClosureReason");

            migrationBuilder.DropColumn(
                name: "ApprovalTypeId",
                table: "ClosureReason");
        }
    }
}
