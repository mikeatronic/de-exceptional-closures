using Microsoft.EntityFrameworkCore.Migrations;

namespace de_exceptional_closures_Infrastructure.Migrations
{
    public partial class AddNUllables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClosureReason_ApprovalType_ApprovalTypeId",
                table: "ClosureReason");

            migrationBuilder.DropForeignKey(
                name: "FK_ClosureReason_ReasonType_ReasonTypeId",
                table: "ClosureReason");

            migrationBuilder.AlterColumn<int>(
                name: "ReasonTypeId",
                table: "ClosureReason",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovalTypeId",
                table: "ClosureReason",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ClosureReason_ApprovalType_ApprovalTypeId",
                table: "ClosureReason",
                column: "ApprovalTypeId",
                principalTable: "ApprovalType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClosureReason_ReasonType_ReasonTypeId",
                table: "ClosureReason",
                column: "ReasonTypeId",
                principalTable: "ReasonType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClosureReason_ApprovalType_ApprovalTypeId",
                table: "ClosureReason");

            migrationBuilder.DropForeignKey(
                name: "FK_ClosureReason_ReasonType_ReasonTypeId",
                table: "ClosureReason");

            migrationBuilder.AlterColumn<int>(
                name: "ReasonTypeId",
                table: "ClosureReason",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ApprovalTypeId",
                table: "ClosureReason",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClosureReason_ApprovalType_ApprovalTypeId",
                table: "ClosureReason",
                column: "ApprovalTypeId",
                principalTable: "ApprovalType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClosureReason_ReasonType_ReasonTypeId",
                table: "ClosureReason",
                column: "ReasonTypeId",
                principalTable: "ReasonType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
