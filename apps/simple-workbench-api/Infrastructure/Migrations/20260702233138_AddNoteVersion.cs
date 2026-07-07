using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleWorkbench.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNoteVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Note",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Note");
        }
    }
}
