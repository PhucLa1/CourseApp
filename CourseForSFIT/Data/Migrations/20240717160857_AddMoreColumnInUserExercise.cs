using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreColumnInUserExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "aliases",
                table: "user_exercise",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "avatar",
                table: "user_exercise",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "run_time",
                table: "user_exercise",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "version",
                table: "user_exercise",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "aliases",
                table: "user_exercise");

            migrationBuilder.DropColumn(
                name: "avatar",
                table: "user_exercise");

            migrationBuilder.DropColumn(
                name: "run_time",
                table: "user_exercise");

            migrationBuilder.DropColumn(
                name: "version",
                table: "user_exercise");
        }
    }
}
