using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RatedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rated",
                table: "RateTransactions");

            migrationBuilder.DropColumn(
                name: "RatedOther",
                table: "RateTransactions");

            migrationBuilder.AddColumn<int>(
                name: "User1IdRated",
                table: "RateTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "User2IdRated",
                table: "RateTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User1IdRated",
                table: "RateTransactions");

            migrationBuilder.DropColumn(
                name: "User2IdRated",
                table: "RateTransactions");

            migrationBuilder.AddColumn<bool>(
                name: "Rated",
                table: "RateTransactions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RatedOther",
                table: "RateTransactions",
                type: "bit",
                nullable: true);
        }
    }
}
