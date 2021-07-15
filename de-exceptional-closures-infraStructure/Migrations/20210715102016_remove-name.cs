using Microsoft.EntityFrameworkCore.Migrations;

namespace de_exceptional_closures_Infrastructure.Migrations
{
    public partial class removename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ReasonType");

            migrationBuilder.InsertData(
                table: "ReasonType",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "Adverse weather" },
                    { 2, "Use as a polling station" },
                    { 3, "Utilities failure (e.g. water, electricity)" },
                    { 4, "Death of a member of staff, pupil or another person working at the school" },
                    { 5, "Other (inc. COVID-19; please enter start and proposed end date)" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ReasonType",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
