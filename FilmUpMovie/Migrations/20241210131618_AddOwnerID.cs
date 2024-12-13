using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmUpMovie.Migrations
{
    public partial class AddOwnerID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Foods",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Foods",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Foods");
        }
    }
}
