using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusConnect.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_AspNetUsers_ApplicationUserId",
                table: "Answers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Answers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_ApplicationUserId1",
                table: "Answers",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_AspNetUsers_ApplicationUserId",
                table: "Answers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_AspNetUsers_ApplicationUserId1",
                table: "Answers",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_AspNetUsers_ApplicationUserId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_AspNetUsers_ApplicationUserId1",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_ApplicationUserId1",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Answers");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_AspNetUsers_ApplicationUserId",
                table: "Answers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
