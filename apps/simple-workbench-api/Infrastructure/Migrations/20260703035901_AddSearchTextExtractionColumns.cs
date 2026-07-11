using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleWorkbench.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchTextExtractionColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentJson",
                table: "Note",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SearchText",
                table: "Note",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentJson",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "SearchText",
                table: "Note");
        }
    }
}
