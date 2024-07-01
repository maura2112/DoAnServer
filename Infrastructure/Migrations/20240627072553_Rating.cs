using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_ProjectStatus_StatusId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Ratings",
                newName: "RateTransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_StatusId",
                table: "Ratings",
                newName: "IX_Ratings_RateTransactionId");

            migrationBuilder.CreateTable(
                name: "RateTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    AcceptedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BidId = table.Column<long>(type: "bigint", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransactionCompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RateTransactions_Bids_BidId",
                        column: x => x.BidId,
                        principalTable: "Bids",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RateTransactions_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RateTransactions_BidId",
                table: "RateTransactions",
                column: "BidId");

            migrationBuilder.CreateIndex(
                name: "IX_RateTransactions_ProjectId",
                table: "RateTransactions",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_RateTransactions_RateTransactionId",
                table: "Ratings",
                column: "RateTransactionId",
                principalTable: "RateTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_RateTransactions_RateTransactionId",
                table: "Ratings");

            migrationBuilder.DropTable(
                name: "RateTransactions");

            migrationBuilder.RenameColumn(
                name: "RateTransactionId",
                table: "Ratings",
                newName: "StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_RateTransactionId",
                table: "Ratings",
                newName: "IX_Ratings_StatusId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Ratings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_ProjectStatus_StatusId",
                table: "Ratings",
                column: "StatusId",
                principalTable: "ProjectStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
