using Microsoft.EntityFrameworkCore.Migrations;

namespace de_exceptional_closures_Infrastructure.Migrations
{
    public partial class AddExtraReasonTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApprovalRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 2,
                column: "ApprovalRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 3,
                column: "ApprovalRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 4,
                column: "ApprovalRequired",
                value: false);

            migrationBuilder.UpdateData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ApprovalRequired", "Description" },
                values: new object[] { true, "Other" });

            migrationBuilder.InsertData(
                table: "ReasonType",
                columns: new[] { "Id", "ApprovalRequired", "Description" },
                values: new object[] { 6, true, "Covid" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApprovalRequired",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 2,
                column: "ApprovalRequired",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 3,
                column: "ApprovalRequired",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 4,
                column: "ApprovalRequired",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReasonType",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ApprovalRequired", "Description" },
                values: new object[] { null, "Other (inc. COVID-19; please enter start and proposed end date)" });
        }
    }
}
