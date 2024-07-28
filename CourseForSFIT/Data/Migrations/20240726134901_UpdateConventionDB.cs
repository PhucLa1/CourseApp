using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConventionDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tag_name",
                table: "tag_exercise",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "tag_name",
                table: "tag_blog",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "exercise_name",
                table: "exercise",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "content_exercise",
                table: "exercise",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "type_name",
                table: "course_type",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "course_name",
                table: "course",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "contest_name",
                table: "contest",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "chapter_name",
                table: "chapter",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "content_blog",
                table: "blog",
                newName: "content");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "tag_exercise",
                newName: "tag_name");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "tag_blog",
                newName: "tag_name");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "exercise",
                newName: "exercise_name");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "exercise",
                newName: "content_exercise");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "course_type",
                newName: "type_name");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "course",
                newName: "course_name");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "contest",
                newName: "contest_name");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "chapter",
                newName: "chapter_name");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "blog",
                newName: "content_blog");
        }
    }
}
