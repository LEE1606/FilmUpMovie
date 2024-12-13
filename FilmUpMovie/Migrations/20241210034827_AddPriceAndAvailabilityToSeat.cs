using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmUpMovie.Migrations
{
    public partial class AddPriceAndAvailabilityToSeat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Seat",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Seat",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Seat");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Seat");
        }
    }
}
