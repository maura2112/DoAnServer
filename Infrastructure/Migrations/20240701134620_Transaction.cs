using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Transaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RateTransactions_Bids_BidId",
                table: "RateTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_RateTransactions_Projects_ProjectId",
                table: "RateTransactions");

            migrationBuilder.DropIndex(
                name: "IX_RateTransactions_BidId",
                table: "RateTransactions");

            migrationBuilder.DropColumn(
                name: "AcceptedDate",
                table: "RateTransactions");

            migrationBuilder.DropColumn(
                name: "BidId",
                table: "RateTransactions");

            migrationBuilder.RenameColumn(
                name: "TransactionCompletedDate",
                table: "RateTransactions",
                newName: "ProjectAcceptedDate");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "RateTransactions",
                newName: "ProjectUserId");

            migrationBuilder.RenameColumn(
                name: "CompletedDate",
                table: "RateTransactions",
                newName: "BidCompletedDate");

            migrationBuilder.RenameIndex(
                name: "IX_RateTransactions_ProjectId",
                table: "RateTransactions",
                newName: "IX_RateTransactions_ProjectUserId");

            migrationBuilder.AlterColumn<string>(
                name: "ReportCode",
                table: "ReportCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<int>(
                name: "BidUserId",
                table: "RateTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Rated",
                table: "RateTransactions",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RateTransactions_BidUserId",
                table: "RateTransactions",
                column: "BidUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RateTransactions_Users_BidUserId",
                table: "RateTransactions",
                column: "BidUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RateTransactions_Users_ProjectUserId",
                table: "RateTransactions",
                column: "ProjectUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RateTransactions_Users_BidUserId",
                table: "RateTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_RateTransactions_Users_ProjectUserId",
                table: "RateTransactions");

            migrationBuilder.DropIndex(
                name: "IX_RateTransactions_BidUserId",
                table: "RateTransactions");

            migrationBuilder.DropColumn(
                name: "BidUserId",
                table: "RateTransactions");

            migrationBuilder.DropColumn(
                name: "Rated",
                table: "RateTransactions");

            migrationBuilder.RenameColumn(
                name: "ProjectUserId",
                table: "RateTransactions",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "ProjectAcceptedDate",
                table: "RateTransactions",
                newName: "TransactionCompletedDate");

            migrationBuilder.RenameColumn(
                name: "BidCompletedDate",
                table: "RateTransactions",
                newName: "CompletedDate");

            migrationBuilder.RenameIndex(
                name: "IX_RateTransactions_ProjectUserId",
                table: "RateTransactions",
                newName: "IX_RateTransactions_ProjectId");

            migrationBuilder.AlterColumn<string>(
                name: "ReportCode",
                table: "ReportCategories",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptedDate",
                table: "RateTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BidId",
                table: "RateTransactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RateTransactions_BidId",
                table: "RateTransactions",
                column: "BidId");

            migrationBuilder.AddForeignKey(
                name: "FK_RateTransactions_Bids_BidId",
                table: "RateTransactions",
                column: "BidId",
                principalTable: "Bids",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RateTransactions_Projects_ProjectId",
                table: "RateTransactions",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
