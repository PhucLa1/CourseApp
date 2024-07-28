using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChunkColumnInLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Lesson",
                newName: "status");

            migrationBuilder.AddColumn<int>(
                name: "chunk_index",
                table: "Lesson",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "total_chunk",
                table: "Lesson",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "chunk_index",
                table: "Lesson");

            migrationBuilder.DropColumn(
                name: "total_chunk",
                table: "Lesson");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Lesson",
                newName: "Status");
        }
    }
}
