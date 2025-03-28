using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceAppProject.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentClassTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "student_class",
                columns: table => new
                {
                    Student_class_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Student_id = table.Column<string>(type: "char(10)", fixedLength: true, maxLength: 10, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Class_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Student_class_id);
                    table.ForeignKey(
                        name: "fk_studentclass_class",
                        column: x => x.Class_id,
                        principalTable: "class",
                        principalColumn: "Class_id");
                    table.ForeignKey(
                        name: "fk_studentclass_student",
                        column: x => x.Student_id,
                        principalTable: "student",
                        principalColumn: "Utd_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "IX_student_class_Class_id",
                table: "student_class",
                column: "Class_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_class_Student_id",
                table: "student_class",
                column: "Student_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_class");
        }
    }
}
