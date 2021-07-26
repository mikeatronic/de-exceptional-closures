using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace de_exceptional_closures_Infrastructure.Migrations
{
    public partial class AddRejectionReasonTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RejectionReasonId",
                table: "ClosureReason",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RejectionReason",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RejectionReason", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RejectionReason",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "School development" },
                    { 2, "Half day" },
                    { 3, "Split site - other site operational" },
                    { 4, "Building work" },
                    { 5, "Move premises" },
                    { 6, "Some classes still in" },
                    { 7, "Planning day" },
                    { 8, "School open for a couple of hours" },
                    { 9, "Staff in school" },
                    { 10, "Pupils in to sit exams" },
                    { 11, "Strike day" },
                    { 12, "Bank or Public Holiday" },
                    { 13, "Wrong date" },
                    { 14, "Not required" },
                    { 15, "Does not meet criteria" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClosureReason_RejectionReasonId",
                table: "ClosureReason",
                column: "RejectionReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClosureReason_RejectionReason_RejectionReasonId",
                table: "ClosureReason",
                column: "RejectionReasonId",
                principalTable: "RejectionReason",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClosureReason_RejectionReason_RejectionReasonId",
                table: "ClosureReason");

            migrationBuilder.DropTable(
                name: "RejectionReason");

            migrationBuilder.DropIndex(
                name: "IX_ClosureReason_RejectionReasonId",
                table: "ClosureReason");

            migrationBuilder.DropColumn(
                name: "RejectionReasonId",
                table: "ClosureReason");
        }
    }
}
