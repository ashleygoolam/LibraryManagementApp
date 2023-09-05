using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class EditBookIssueModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowersId",
                table: "BookIssue");

            migrationBuilder.DropColumn(
                name: "BorrowingLibrariansId",
                table: "BookIssue");

            migrationBuilder.DropColumn(
                name: "ReturningLibrariansId",
                table: "BookIssue");

            migrationBuilder.AddColumn<string>(
                name: "BorrowersName",
                table: "BookIssue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BorrowingLibrariansName",
                table: "BookIssue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturningLibrariansName",
                table: "BookIssue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Author",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Biography",
                table: "Author",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowersName",
                table: "BookIssue");

            migrationBuilder.DropColumn(
                name: "BorrowingLibrariansName",
                table: "BookIssue");

            migrationBuilder.DropColumn(
                name: "ReturningLibrariansName",
                table: "BookIssue");

            migrationBuilder.AddColumn<int>(
                name: "BorrowersId",
                table: "BookIssue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BorrowingLibrariansId",
                table: "BookIssue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReturningLibrariansId",
                table: "BookIssue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Author",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Biography",
                table: "Author",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
