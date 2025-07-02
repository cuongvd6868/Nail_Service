using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nail_Service.Migrations
{
    /// <inheritdoc />
    public partial class Initdb3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "NailServices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "NailServices");
        }
    }
}
