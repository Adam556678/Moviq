using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheaterService.Migrations
{
    /// <inheritdoc />
    public partial class AddingRowsColsToHalls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalSeats",
                table: "Halls",
                newName: "NumRows");

            migrationBuilder.AddColumn<int>(
                name: "NumColumns",
                table: "Halls",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumColumns",
                table: "Halls");

            migrationBuilder.RenameColumn(
                name: "NumRows",
                table: "Halls",
                newName: "TotalSeats");
        }
    }
}
