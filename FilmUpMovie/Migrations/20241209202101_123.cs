using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmUpMovie.Migrations
{
    public partial class _123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StaffAge = table.Column<int>(type: "int", nullable: false),
                    StaffEmail = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StaffPhoneNumber = table.Column<int>(type: "int", nullable: false),
                    StaffAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffWorkDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LeaveApplications",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveAppTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    LeaveAppDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaveAppReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StaffID = table.Column<int>(type: "int", nullable: false),
                    OwnerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveApplications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LeaveApplications_Staffs_StaffID",
                        column: x => x.StaffID,
                        principalTable: "Staffs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalWorkTime = table.Column<double>(type: "float", nullable: false),
                    PayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayAmount = table.Column<double>(type: "float", nullable: false),
                    StaffID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Payments_Staffs_StaffID",
                        column: x => x.StaffID,
                        principalTable: "Staffs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_StaffID",
                table: "LeaveApplications",
                column: "StaffID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_StaffID",
                table: "Payments",
                column: "StaffID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveApplications");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Staffs");
        }
    }
}
