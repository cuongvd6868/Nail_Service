using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nail_Service.Migrations
{
    /// <inheritdoc />
    public partial class Initdb4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_BookingNailId",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookingNailId",
                table: "Reviews",
                column: "BookingNailId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_BookingNailId",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookingNailId",
                table: "Reviews",
                column: "BookingNailId");
        }
    }
}
