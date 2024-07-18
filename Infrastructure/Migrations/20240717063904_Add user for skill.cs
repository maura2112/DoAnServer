using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adduserforskill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Skills",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Skills",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Skills",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CreatedBy",
                table: "Skills",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Users_CreatedBy",
                table: "Skills",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Users_CreatedBy",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_CreatedBy",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Skills");
        }
    }
}
