using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmUpMovie.Migrations
{
    public partial class AdBeveraOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Combos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Combos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Beverages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Beverages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Combos");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Combos");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Beverages");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Beverages");
        }
    }
}
