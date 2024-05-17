using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Mediaid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_MediaFile_MediaFileId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_MediaFileId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "MediaFileId",
                table: "Projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_MediaFile_MediaFileId1",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_MediaFileId1",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MediaFileId1",
                table: "Projects");

            migrationBuilder.AlterColumn<long>(
                name: "MediaFileId",
                table: "Projects",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediaId",
                table: "Projects",
                type: "int",
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
    }
}
