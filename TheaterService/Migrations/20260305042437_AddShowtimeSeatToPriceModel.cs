using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheaterService.Migrations
{
    /// <inheritdoc />
    public partial class AddShowtimeSeatToPriceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatType",
                table: "Prices");

            migrationBuilder.AddColumn<Guid>(
                name: "ShowtimeSeatId",
                table: "Prices",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Prices_ShowtimeSeatId",
                table: "Prices",
                column: "ShowtimeSeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_ShowtimeSeats_ShowtimeSeatId",
                table: "Prices",
                column: "ShowtimeSeatId",
                principalTable: "ShowtimeSeats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_ShowtimeSeats_ShowtimeSeatId",
                table: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Prices_ShowtimeSeatId",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "ShowtimeSeatId",
                table: "Prices");

            migrationBuilder.AddColumn<int>(
                name: "SeatType",
                table: "Prices",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
