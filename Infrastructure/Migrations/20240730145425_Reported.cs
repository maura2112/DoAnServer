using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Reported : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserReportedId",
                table: "UserReports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserReports_UserReportedId",
                table: "UserReports",
                column: "UserReportedId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReports_Users_UserReportedId",
                table: "UserReports",
                column: "UserReportedId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReports_Users_UserReportedId",
                table: "UserReports");

            migrationBuilder.DropIndex(
                name: "IX_UserReports_UserReportedId",
                table: "UserReports");

            migrationBuilder.DropColumn(
                name: "UserReportedId",
                table: "UserReports");
        }
    }
}
