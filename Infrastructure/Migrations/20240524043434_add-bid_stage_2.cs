using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addbid_stage_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Decription",
                table: "BidStages");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "BidStages");

            migrationBuilder.DropColumn(
                name: "NumberStage",
                table: "BidStages");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "BidStages");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "BidStages",
                newName: "TotalOfStage");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "BidStages",
                newName: "IsAccepted");

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BidStageId = table.Column<int>(type: "int", nullable: false),
                    NumberStage = table.Column<int>(type: "int", nullable: false),
                    Decription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stages_BidStages_BidStageId",
                        column: x => x.BidStageId,
                        principalTable: "BidStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stages_BidStageId",
                table: "Stages",
                column: "BidStageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.RenameColumn(
                name: "TotalOfStage",
                table: "BidStages",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "IsAccepted",
                table: "BidStages",
                newName: "IsCompleted");

            migrationBuilder.AddColumn<string>(
                name: "Decription",
                table: "BidStages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "BidStages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "NumberStage",
                table: "BidStages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "BidStages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
