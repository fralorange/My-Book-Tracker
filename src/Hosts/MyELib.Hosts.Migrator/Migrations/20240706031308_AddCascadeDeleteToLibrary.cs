using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyELib.Hosts.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteToLibrary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Library_LibraryId",
                table: "Document");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Library_LibraryId",
                table: "Document",
                column: "LibraryId",
                principalTable: "Library",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Library_LibraryId",
                table: "Document");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Library_LibraryId",
                table: "Document",
                column: "LibraryId",
                principalTable: "Library",
                principalColumn: "Id");
        }
    }
}
