using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class authorBookTableId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Book",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Author",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Book",
                newName: "BookId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Author",
                newName: "AuthorId");
        }
    }
}
