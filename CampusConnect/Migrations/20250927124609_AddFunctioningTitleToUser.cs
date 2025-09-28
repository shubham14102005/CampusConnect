using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddFunctioningTitleToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FunctioningTitle",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FunctioningTitle",
                table: "AspNetUsers");
        }
    }
}
