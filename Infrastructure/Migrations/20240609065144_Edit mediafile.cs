using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Editmediafile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFile_MediaFolder_FolderId",
                table: "MediaFile");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaFile_Users_UserId",
                table: "MediaFile");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "MediaFile");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "MediaFile");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "MediaFile",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FolderId",
                table: "MediaFile",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MediaFile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "MediaFile",
                type: "nvarchar(max)",
                nullable: true);


            migrationBuilder.AddForeignKey(
                name: "FK_MediaFile_MediaFolder_FolderId",
                table: "MediaFile",
                column: "FolderId",
                principalTable: "MediaFolder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFile_Users_UserId",
                table: "MediaFile",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFile_MediaFolder_FolderId",
                table: "MediaFile");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaFile_Users_UserId",
                table: "MediaFile");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MediaFile");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "MediaFile");


            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "MediaFile",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FolderId",
                table: "MediaFile",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "MediaFile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "MediaFile",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFile_MediaFolder_FolderId",
                table: "MediaFile",
                column: "FolderId",
                principalTable: "MediaFolder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFile_Users_UserId",
                table: "MediaFile",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
