using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class editTablesId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublisherId",
                table: "Publisher",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IssueId",
                table: "BookIssue",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Publisher",
                newName: "PublisherId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BookIssue",
                newName: "IssueId");
        }
    }
}
