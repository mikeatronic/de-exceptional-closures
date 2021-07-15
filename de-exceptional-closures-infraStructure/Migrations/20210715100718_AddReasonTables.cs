using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace de_exceptional_closures_Infrastructure.Migrations
{
    public partial class AddReasonTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReasonType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReasonType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClosureReason",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    InstitutionName = table.Column<string>(nullable: true),
                    Srn = table.Column<string>(nullable: true),
                    DateFrom = table.Column<DateTime>(nullable: false),
                    DateTo = table.Column<DateTime>(nullable: true),
                    ReasonTypeId = table.Column<int>(nullable: false),
                    OtherReason = table.Column<string>(nullable: true),
                    Approved = table.Column<bool>(nullable: false),
                    ApprovalDate = table.Column<DateTime>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosureReason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClosureReason_ReasonType_ReasonTypeId",
                        column: x => x.ReasonTypeId,
                        principalTable: "ReasonType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClosureReason_ReasonTypeId",
                table: "ClosureReason",
                column: "ReasonTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClosureReason");

            migrationBuilder.DropTable(
                name: "ReasonType");
        }
    }
}
