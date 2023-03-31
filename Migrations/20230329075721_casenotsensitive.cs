using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Migrations
{
    public partial class casenotsensitive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_TodoUsers_UserID",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "TodoItems",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TodoItems_UserID",
                table: "TodoItems",
                newName: "IX_TodoItems_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_TodoUsers_UserId",
                table: "TodoItems",
                column: "UserId",
                principalTable: "TodoUsers",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_TodoUsers_UserId",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TodoItems",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_TodoItems_UserId",
                table: "TodoItems",
                newName: "IX_TodoItems_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_TodoUsers_UserID",
                table: "TodoItems",
                column: "UserID",
                principalTable: "TodoUsers",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
