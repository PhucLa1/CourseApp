using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_result");

            migrationBuilder.DropColumn(
                name: "tag",
                table: "exercise");

            migrationBuilder.CreateTable(
                name: "blog_has_tag",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tag_blog_id = table.Column<int>(type: "int", nullable: false),
                    blog_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    updated_by = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_has_tag", x => x.id);
                    table.ForeignKey(
                        name: "FK_blog_has_tag_blog_blog_id",
                        column: x => x.blog_id,
                        principalTable: "blog",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_blog_has_tag_tag_blog_tag_blog_id",
                        column: x => x.tag_blog_id,
                        principalTable: "tag_blog",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lesson",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lesson_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    is_lock = table.Column<bool>(type: "bit", nullable: false),
                    chapter_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    updated_by = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lesson", x => x.id);
                    table.ForeignKey(
                        name: "FK_Lesson_chapter_chapter_id",
                        column: x => x.chapter_id,
                        principalTable: "chapter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_join_contest_id",
                table: "user_join",
                column: "contest_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_join_user_id",
                table: "user_join",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_exercise_exercise_id",
                table: "user_exercise",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_exercise_user_id",
                table: "user_exercise",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_case_exercise_id",
                table: "test_case",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_course_lesson_id",
                table: "lesson_course",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_course_user_id",
                table: "lesson_course",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_comment_lesson_id",
                table: "lesson_comment",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_has_tag_exercise_id",
                table: "exercise_has_tag",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_has_tag_tag_exercise_id",
                table: "exercise_has_tag",
                column: "tag_exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_exercise_comment_exercise_id",
                table: "exercise_comment",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_user_course_id",
                table: "course_user",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_user_user_id",
                table: "course_user",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_comment_course_id",
                table: "course_comment",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_course_type_id",
                table: "course",
                column: "course_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_contest_exercise_contest_id",
                table: "contest_exercise",
                column: "contest_id");

            migrationBuilder.CreateIndex(
                name: "IX_contest_exercise_exercise_id",
                table: "contest_exercise",
                column: "exercise_id");

            migrationBuilder.CreateIndex(
                name: "IX_chapter_course_id",
                table: "chapter",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_blog_comment_blog_id",
                table: "blog_comment",
                column: "blog_id");

            migrationBuilder.CreateIndex(
                name: "IX_blog_has_tag_blog_id",
                table: "blog_has_tag",
                column: "blog_id");

            migrationBuilder.CreateIndex(
                name: "IX_blog_has_tag_tag_blog_id",
                table: "blog_has_tag",
                column: "tag_blog_id");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_chapter_id",
                table: "Lesson",
                column: "chapter_id");

            migrationBuilder.AddForeignKey(
                name: "FK_blog_comment_blog_blog_id",
                table: "blog_comment",
                column: "blog_id",
                principalTable: "blog",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_chapter_course_course_id",
                table: "chapter",
                column: "course_id",
                principalTable: "course",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_contest_exercise_contest_contest_id",
                table: "contest_exercise",
                column: "contest_id",
                principalTable: "contest",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_contest_exercise_exercise_exercise_id",
                table: "contest_exercise",
                column: "exercise_id",
                principalTable: "exercise",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_course_course_type_course_type_id",
                table: "course",
                column: "course_type_id",
                principalTable: "course_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_course_comment_course_course_id",
                table: "course_comment",
                column: "course_id",
                principalTable: "course",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_course_user_course_course_id",
                table: "course_user",
                column: "course_id",
                principalTable: "course",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_course_user_user_user_id",
                table: "course_user",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_exercise_comment_exercise_exercise_id",
                table: "exercise_comment",
                column: "exercise_id",
                principalTable: "exercise",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_exercise_has_tag_exercise_exercise_id",
                table: "exercise_has_tag",
                column: "exercise_id",
                principalTable: "exercise",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_exercise_has_tag_tag_exercise_tag_exercise_id",
                table: "exercise_has_tag",
                column: "tag_exercise_id",
                principalTable: "tag_exercise",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_comment_Lesson_lesson_id",
                table: "lesson_comment",
                column: "lesson_id",
                principalTable: "Lesson",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_course_Lesson_lesson_id",
                table: "lesson_course",
                column: "lesson_id",
                principalTable: "Lesson",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_course_user_user_id",
                table: "lesson_course",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_test_case_exercise_exercise_id",
                table: "test_case",
                column: "exercise_id",
                principalTable: "exercise",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_exercise_exercise_exercise_id",
                table: "user_exercise",
                column: "exercise_id",
                principalTable: "exercise",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_exercise_user_user_id",
                table: "user_exercise",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_join_contest_contest_id",
                table: "user_join",
                column: "contest_id",
                principalTable: "contest",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_join_user_user_id",
                table: "user_join",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_blog_comment_blog_blog_id",
                table: "blog_comment");

            migrationBuilder.DropForeignKey(
                name: "FK_chapter_course_course_id",
                table: "chapter");

            migrationBuilder.DropForeignKey(
                name: "FK_contest_exercise_contest_contest_id",
                table: "contest_exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_contest_exercise_exercise_exercise_id",
                table: "contest_exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_course_course_type_course_type_id",
                table: "course");

            migrationBuilder.DropForeignKey(
                name: "FK_course_comment_course_course_id",
                table: "course_comment");

            migrationBuilder.DropForeignKey(
                name: "FK_course_user_course_course_id",
                table: "course_user");

            migrationBuilder.DropForeignKey(
                name: "FK_course_user_user_user_id",
                table: "course_user");

            migrationBuilder.DropForeignKey(
                name: "FK_exercise_comment_exercise_exercise_id",
                table: "exercise_comment");

            migrationBuilder.DropForeignKey(
                name: "FK_exercise_has_tag_exercise_exercise_id",
                table: "exercise_has_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_exercise_has_tag_tag_exercise_tag_exercise_id",
                table: "exercise_has_tag");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_comment_Lesson_lesson_id",
                table: "lesson_comment");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_course_Lesson_lesson_id",
                table: "lesson_course");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_course_user_user_id",
                table: "lesson_course");

            migrationBuilder.DropForeignKey(
                name: "FK_test_case_exercise_exercise_id",
                table: "test_case");

            migrationBuilder.DropForeignKey(
                name: "FK_user_exercise_exercise_exercise_id",
                table: "user_exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_user_exercise_user_user_id",
                table: "user_exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_user_join_contest_contest_id",
                table: "user_join");

            migrationBuilder.DropForeignKey(
                name: "FK_user_join_user_user_id",
                table: "user_join");

            migrationBuilder.DropTable(
                name: "blog_has_tag");

            migrationBuilder.DropTable(
                name: "Lesson");

            migrationBuilder.DropIndex(
                name: "IX_user_join_contest_id",
                table: "user_join");

            migrationBuilder.DropIndex(
                name: "IX_user_join_user_id",
                table: "user_join");

            migrationBuilder.DropIndex(
                name: "IX_user_exercise_exercise_id",
                table: "user_exercise");

            migrationBuilder.DropIndex(
                name: "IX_user_exercise_user_id",
                table: "user_exercise");

            migrationBuilder.DropIndex(
                name: "IX_test_case_exercise_id",
                table: "test_case");

            migrationBuilder.DropIndex(
                name: "IX_lesson_course_lesson_id",
                table: "lesson_course");

            migrationBuilder.DropIndex(
                name: "IX_lesson_course_user_id",
                table: "lesson_course");

            migrationBuilder.DropIndex(
                name: "IX_lesson_comment_lesson_id",
                table: "lesson_comment");

            migrationBuilder.DropIndex(
                name: "IX_exercise_has_tag_exercise_id",
                table: "exercise_has_tag");

            migrationBuilder.DropIndex(
                name: "IX_exercise_has_tag_tag_exercise_id",
                table: "exercise_has_tag");

            migrationBuilder.DropIndex(
                name: "IX_exercise_comment_exercise_id",
                table: "exercise_comment");

            migrationBuilder.DropIndex(
                name: "IX_course_user_course_id",
                table: "course_user");

            migrationBuilder.DropIndex(
                name: "IX_course_user_user_id",
                table: "course_user");

            migrationBuilder.DropIndex(
                name: "IX_course_comment_course_id",
                table: "course_comment");

            migrationBuilder.DropIndex(
                name: "IX_course_course_type_id",
                table: "course");

            migrationBuilder.DropIndex(
                name: "IX_contest_exercise_contest_id",
                table: "contest_exercise");

            migrationBuilder.DropIndex(
                name: "IX_contest_exercise_exercise_id",
                table: "contest_exercise");

            migrationBuilder.DropIndex(
                name: "IX_chapter_course_id",
                table: "chapter");

            migrationBuilder.DropIndex(
                name: "IX_blog_comment_blog_id",
                table: "blog_comment");

            migrationBuilder.AddColumn<string>(
                name: "tag",
                table: "exercise",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "user_result",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    is_pass = table.Column<bool>(type: "bit", nullable: false),
                    test_case_id = table.Column<int>(type: "int", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<int>(type: "int", nullable: false),
                    user_exercise_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_result", x => x.id);
                });
        }
    }
}
