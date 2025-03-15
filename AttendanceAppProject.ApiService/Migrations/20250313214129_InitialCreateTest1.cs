using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceAppProject.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateTest1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "professor",
                columns: table => new
                {
                    Utd_id = table.Column<string>(type: "char(10)", fixedLength: true, maxLength: 10, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    First_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Last_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Utd_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    Utd_id = table.Column<string>(type: "char(10)", fixedLength: true, maxLength: 10, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    First_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Last_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Username = table.Column<string>(type: "char(9)", fixedLength: true, maxLength: 9, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Utd_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "class",
                columns: table => new
                {
                    Class_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Prof_utd_id = table.Column<string>(type: "char(10)", fixedLength: true, maxLength: 10, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Class_prefix = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Class_number = table.Column<string>(type: "char(8)", fixedLength: true, maxLength: 8, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Class_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    End_date = table.Column<DateOnly>(type: "date", nullable: true),
                    Start_time = table.Column<TimeOnly>(type: "time", nullable: true),
                    End_time = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Class_id);
                    table.ForeignKey(
                        name: "fk_class_profId",
                        column: x => x.Prof_utd_id,
                        principalTable: "professor",
                        principalColumn: "Utd_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "attendance_instance",
                columns: table => new
                {
                    Attendance_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Student_id = table.Column<string>(type: "char(10)", fixedLength: true, maxLength: 10, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Class_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Ip_address = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Is_late = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Excused_absence = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Date_time = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Attendance_id);
                    table.ForeignKey(
                        name: "fk_attendanceInstance_classId",
                        column: x => x.Class_id,
                        principalTable: "class",
                        principalColumn: "Class_id");
                    table.ForeignKey(
                        name: "fk_attendanceInstance_studentId",
                        column: x => x.Student_id,
                        principalTable: "student",
                        principalColumn: "Utd_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "class_schedule",
                columns: table => new
                {
                    Class_schedule_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Class_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Day_of_Week = table.Column<string>(type: "enum('Monday','Tuesday','Wednesday','Thursday','Friday','Saturday','Sunday')", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Class_schedule_id);
                    table.ForeignKey(
                        name: "fk_classSchedule_classId",
                        column: x => x.Class_id,
                        principalTable: "class",
                        principalColumn: "Class_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "passwords",
                columns: table => new
                {
                    Password_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Class_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date_assigned = table.Column<DateOnly>(type: "date", nullable: true),
                    Password_text = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Password_id);
                    table.ForeignKey(
                        name: "fk_passwords_classId",
                        column: x => x.Class_id,
                        principalTable: "class",
                        principalColumn: "Class_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "quiz_instance",
                columns: table => new
                {
                    Quiz_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Class_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Start_time = table.Column<DateTime>(type: "timestamp", nullable: true),
                    End_time = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Quiz_id);
                    table.ForeignKey(
                        name: "fk_quizInstance_classId",
                        column: x => x.Class_id,
                        principalTable: "class",
                        principalColumn: "Class_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "quiz_questions",
                columns: table => new
                {
                    Question_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Quiz_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Correct_answer = table.Column<string>(type: "char(1)", fixedLength: true, maxLength: 1, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Question_text = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Answer_a = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Answer_b = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Answer_c = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Answer_d = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Question_id);
                    table.ForeignKey(
                        name: "fk_quizQuestions_quizId",
                        column: x => x.Quiz_id,
                        principalTable: "quiz_instance",
                        principalColumn: "Quiz_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "quiz_responses",
                columns: table => new
                {
                    Response_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Student_id = table.Column<string>(type: "char(10)", fixedLength: true, maxLength: 10, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quiz_question_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Quiz_instance_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Response_id);
                    table.ForeignKey(
                        name: "fk_quizResponses_quizInstanceid",
                        column: x => x.Quiz_instance_id,
                        principalTable: "quiz_instance",
                        principalColumn: "Quiz_id");
                    table.ForeignKey(
                        name: "fk_quizResponses_quizQuestionId",
                        column: x => x.Quiz_question_id,
                        principalTable: "quiz_questions",
                        principalColumn: "Question_id");
                    table.ForeignKey(
                        name: "fk_quizResponses_studentId",
                        column: x => x.Student_id,
                        principalTable: "student",
                        principalColumn: "Utd_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "fk_attendanceInstance_classId",
                table: "attendance_instance",
                column: "Class_id");

            migrationBuilder.CreateIndex(
                name: "fk_attendanceInstance_studentId",
                table: "attendance_instance",
                column: "Student_id");

            migrationBuilder.CreateIndex(
                name: "fk_class_profId",
                table: "class",
                column: "Prof_utd_id");

            migrationBuilder.CreateIndex(
                name: "fk_classSchedule_classId",
                table: "class_schedule",
                column: "Class_id");

            migrationBuilder.CreateIndex(
                name: "fk_passwords_classId",
                table: "passwords",
                column: "Class_id");

            migrationBuilder.CreateIndex(
                name: "fk_quizInstance_classId",
                table: "quiz_instance",
                column: "Class_id");

            migrationBuilder.CreateIndex(
                name: "fk_quizQuestions_quizId",
                table: "quiz_questions",
                column: "Quiz_id");

            migrationBuilder.CreateIndex(
                name: "fk_quizResponses_quizInstanceid",
                table: "quiz_responses",
                column: "Quiz_instance_id");

            migrationBuilder.CreateIndex(
                name: "fk_quizResponses_quizQuestionId",
                table: "quiz_responses",
                column: "Quiz_question_id");

            migrationBuilder.CreateIndex(
                name: "fk_quizResponses_studentId",
                table: "quiz_responses",
                column: "Student_id");

            migrationBuilder.CreateIndex(
                name: "Username",
                table: "student",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attendance_instance");

            migrationBuilder.DropTable(
                name: "class_schedule");

            migrationBuilder.DropTable(
                name: "passwords");

            migrationBuilder.DropTable(
                name: "quiz_responses");

            migrationBuilder.DropTable(
                name: "quiz_questions");

            migrationBuilder.DropTable(
                name: "student");

            migrationBuilder.DropTable(
                name: "quiz_instance");

            migrationBuilder.DropTable(
                name: "class");

            migrationBuilder.DropTable(
                name: "professor");
        }
    }
}
