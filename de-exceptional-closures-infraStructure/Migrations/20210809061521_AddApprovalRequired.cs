using Microsoft.EntityFrameworkCore.Migrations;

namespace de_exceptional_closures_Infrastructure.Migrations
{
    public partial class AddApprovalRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ApprovalRequired",
                table: "ReasonType",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalRequired",
                table: "ReasonType");
        }
    }
}
