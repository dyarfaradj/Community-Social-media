using Microsoft.EntityFrameworkCore.Migrations;

namespace Distribuerade_System_Labb_2.Migrations
{
    public partial class updatedmessage5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverUserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ReceiverUserId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "ReceiverUserId",
                table: "Messages",
                newName: "ReceiverId");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Messages",
                newName: "ReceiverUserId");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverUserId",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverUserId",
                table: "Messages",
                column: "ReceiverUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverUserId",
                table: "Messages",
                column: "ReceiverUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
