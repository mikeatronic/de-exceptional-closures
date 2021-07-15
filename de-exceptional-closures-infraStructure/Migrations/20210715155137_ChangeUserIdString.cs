using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace de_exceptional_closures_Infrastructure.Migrations
{
    public partial class ChangeUserIdString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ClosureReason",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ClosureReason",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
