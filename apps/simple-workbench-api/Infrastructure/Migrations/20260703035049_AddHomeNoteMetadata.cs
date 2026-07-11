using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleWorkbench.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeNoteMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSaved",
                table: "Note",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastViewedAt",
                table: "Note",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSaved",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "LastViewedAt",
                table: "Note");
        }
    }
}
