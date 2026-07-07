using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleWorkbench.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInlineSecrets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InlineSecret",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    NoteId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    SecretKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SecretValue = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InlineSecret", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InlineSecret_NoteId_SecretKey",
                table: "InlineSecret",
                columns: new[] { "NoteId", "SecretKey" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InlineSecret");
        }
    }
}
