using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusConnect.Migrations
{
    /// <inheritdoc />
    public partial class initially : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerVotes_AspNetUsers_ApplicationUserId",
                table: "AnswerVotes");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerVotes_AspNetUsers_ApplicationUserId",
                table: "AnswerVotes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerVotes_AspNetUsers_ApplicationUserId",
                table: "AnswerVotes");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerVotes_AspNetUsers_ApplicationUserId",
                table: "AnswerVotes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
