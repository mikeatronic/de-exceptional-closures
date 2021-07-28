using Microsoft.EntityFrameworkCore.Migrations;

namespace de_exceptional_closures_Infrastructure.Migrations
{
    public partial class AddUserEmailToClosureReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "ClosureReason",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "ClosureReason");
        }
    }
}
