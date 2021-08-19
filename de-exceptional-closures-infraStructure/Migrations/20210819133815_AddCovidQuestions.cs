using Microsoft.EntityFrameworkCore.Migrations;

namespace de_exceptional_closures_Infrastructure.Migrations
{
    public partial class AddCovidQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CovidQ1",
                table: "ClosureReason",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CovidQ2",
                table: "ClosureReason",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CovidQ3",
                table: "ClosureReason",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CovidQ4",
                table: "ClosureReason",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CovidQ5",
                table: "ClosureReason",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CovidQ1",
                table: "ClosureReason");

            migrationBuilder.DropColumn(
                name: "CovidQ2",
                table: "ClosureReason");

            migrationBuilder.DropColumn(
                name: "CovidQ3",
                table: "ClosureReason");

            migrationBuilder.DropColumn(
                name: "CovidQ4",
                table: "ClosureReason");

            migrationBuilder.DropColumn(
                name: "CovidQ5",
                table: "ClosureReason");
        }
    }
}
