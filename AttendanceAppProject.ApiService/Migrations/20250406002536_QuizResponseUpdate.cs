using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceAppProject.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class QuizResponseUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer_a",
                table: "quiz_questions");

            migrationBuilder.DropColumn(
                name: "Answer_b",
                table: "quiz_questions");

            migrationBuilder.DropColumn(
                name: "Answer_c",
                table: "quiz_questions");

            migrationBuilder.DropColumn(
                name: "Answer_d",
                table: "quiz_questions");

            migrationBuilder.DropColumn(
                name: "Correct_answer",
                table: "quiz_questions");

            migrationBuilder.CreateTable(
                name: "quiz_answers",
                columns: table => new
                {
                    Answer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Question_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Quiz_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Answer_text = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Answer_isCorrect = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Answer_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "fk_questionId_quizQuestions",
                table: "quiz_answers",
                column: "Question_id");

            migrationBuilder.CreateIndex(
                name: "fk_quizId_quizInstance",
                table: "quiz_answers",
                column: "Quiz_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "quiz_answers");

            migrationBuilder.AddColumn<string>(
                name: "Answer_a",
                table: "quiz_questions",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Answer_b",
                table: "quiz_questions",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Answer_c",
                table: "quiz_questions",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Answer_d",
                table: "quiz_questions",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Correct_answer",
                table: "quiz_questions",
                type: "char(1)",
                fixedLength: true,
                maxLength: 1,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
