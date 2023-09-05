using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class editTableColums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookInstance_BookIssue_BookIssueId",
                table: "BookInstance");

            migrationBuilder.DropIndex(
                name: "IX_BookInstance_BookIssueId",
                table: "BookInstance");

            migrationBuilder.RenameColumn(
                name: "BookIssueId",
                table: "BookInstance",
                newName: "bookAvailability");

            migrationBuilder.RenameColumn(
                name: "CurrentAmount",
                table: "Book",
                newName: "AvailableAmount");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Publisher",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BookInstanceId",
                table: "BookIssue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "BookIssue",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "bookReturnStatus",
                table: "BookIssue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Book",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookIssue_BookInstanceId",
                table: "BookIssue",
                column: "BookInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookIssue_BookInstance_BookInstanceId",
                table: "BookIssue",
                column: "BookInstanceId",
                principalTable: "BookInstance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookIssue_BookInstance_BookInstanceId",
                table: "BookIssue");

            migrationBuilder.DropIndex(
                name: "IX_BookIssue_BookInstanceId",
                table: "BookIssue");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Publisher");

            migrationBuilder.DropColumn(
                name: "BookInstanceId",
                table: "BookIssue");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "BookIssue");

            migrationBuilder.DropColumn(
                name: "bookReturnStatus",
                table: "BookIssue");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "bookAvailability",
                table: "BookInstance",
                newName: "BookIssueId");

            migrationBuilder.RenameColumn(
                name: "AvailableAmount",
                table: "Book",
                newName: "CurrentAmount");

            migrationBuilder.CreateIndex(
                name: "IX_BookInstance_BookIssueId",
                table: "BookInstance",
                column: "BookIssueId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookInstance_BookIssue_BookIssueId",
                table: "BookInstance",
                column: "BookIssueId",
                principalTable: "BookIssue",
                principalColumn: "IssueId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
