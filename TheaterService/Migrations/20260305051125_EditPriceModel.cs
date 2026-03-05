using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheaterService.Migrations
{
    /// <inheritdoc />
    public partial class EditPriceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidFrom",
                table: "Prices");

            migrationBuilder.RenameColumn(
                name: "ValidTo",
                table: "Prices",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Prices",
                newName: "ValidTo");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidFrom",
                table: "Prices",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
