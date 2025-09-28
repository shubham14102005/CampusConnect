using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddUpvoteDownvoteCountToAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Answers",
                newName: "UpvoteCount");

            migrationBuilder.AddColumn<int>(
                name: "DownvoteCount",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownvoteCount",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "UpvoteCount",
                table: "Answers",
                newName: "Score");
        }
    }
}
