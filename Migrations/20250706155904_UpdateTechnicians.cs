using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nail_Service.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTechnicians : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NailTechnicians_AspNetUsers_AppUserId",
                table: "NailTechnicians");

            migrationBuilder.DropForeignKey(
                name: "FK_NailTechnicians_NailSalons_NailSalonId",
                table: "NailTechnicians");

            migrationBuilder.DropIndex(
                name: "IX_NailTechnicians_AppUserId",
                table: "NailTechnicians");

            migrationBuilder.AlterColumn<int>(
                name: "YearsOfExperience",
                table: "NailTechnicians",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "NailTechnicians",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Specialties",
                table: "NailTechnicians",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "NailTechnicians",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfReviews",
                table: "NailTechnicians",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "NailSalonId",
                table: "NailTechnicians",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "NailTechnicians",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "NailTechnicians",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<double>(
                name: "AverageRating",
                table: "NailTechnicians",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "NailTechnicians",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "NailTechnicians",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NailTechnicians_AppUserId",
                table: "NailTechnicians",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_NailTechnicians_AspNetUsers_AppUserId",
                table: "NailTechnicians",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NailTechnicians_NailSalons_NailSalonId",
                table: "NailTechnicians",
                column: "NailSalonId",
                principalTable: "NailSalons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NailTechnicians_AspNetUsers_AppUserId",
                table: "NailTechnicians");

            migrationBuilder.DropForeignKey(
                name: "FK_NailTechnicians_NailSalons_NailSalonId",
                table: "NailTechnicians");

            migrationBuilder.DropIndex(
                name: "IX_NailTechnicians_AppUserId",
                table: "NailTechnicians");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "NailTechnicians");

            migrationBuilder.AlterColumn<int>(
                name: "YearsOfExperience",
                table: "NailTechnicians",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "NailTechnicians",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Specialties",
                table: "NailTechnicians",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "NailTechnicians",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfReviews",
                table: "NailTechnicians",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NailSalonId",
                table: "NailTechnicians",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "NailTechnicians",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "NailTechnicians",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "AverageRating",
                table: "NailTechnicians",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "NailTechnicians",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NailTechnicians_AppUserId",
                table: "NailTechnicians",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NailTechnicians_AspNetUsers_AppUserId",
                table: "NailTechnicians",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NailTechnicians_NailSalons_NailSalonId",
                table: "NailTechnicians",
                column: "NailSalonId",
                principalTable: "NailSalons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
