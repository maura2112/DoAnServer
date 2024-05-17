using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deleteMediaFiel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_MediaFile_MediaFileId1",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_MediaFileId1",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MediaFileId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MediaFileId1",
                table: "Projects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MediaFileId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MediaFileId1",
                table: "Projects",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_MediaFileId1",
                table: "Projects",
                column: "MediaFileId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_MediaFile_MediaFileId1",
                table: "Projects",
                column: "MediaFileId1",
                principalTable: "MediaFile",
                principalColumn: "Id");
        }
    }
}
