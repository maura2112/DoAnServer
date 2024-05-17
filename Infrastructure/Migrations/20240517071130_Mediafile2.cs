using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Mediafile2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MediaFileId",
                table: "Projects",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_MediaFileId",
                table: "Projects",
                column: "MediaFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_MediaFile_MediaFileId",
                table: "Projects",
                column: "MediaFileId",
                principalTable: "MediaFile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_MediaFile_MediaFileId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_MediaFileId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MediaFileId",
                table: "Projects");
        }
    }
}
