using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAcces.Migrations
{
    public partial class fix45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "UserId",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_UserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
