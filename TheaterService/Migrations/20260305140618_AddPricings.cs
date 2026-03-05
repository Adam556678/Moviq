using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheaterService.Migrations
{
    /// <inheritdoc />
    public partial class AddPricings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HallPricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HallType = table.Column<int>(type: "integer", nullable: false),
                    Multiplier = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallPricing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeatPricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SeatType = table.Column<int>(type: "integer", nullable: false),
                    BasePrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatPricing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimePricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartHour = table.Column<string>(type: "text", nullable: false),
                    EndHour = table.Column<string>(type: "text", nullable: false),
                    Multiplier = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimePricing", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HallPricing");

            migrationBuilder.DropTable(
                name: "SeatPricing");

            migrationBuilder.DropTable(
                name: "TimePricing");
        }
    }
}
