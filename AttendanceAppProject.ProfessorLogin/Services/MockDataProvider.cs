// Canh Nguyen

using System;
using System.Collections.Generic;
using AttendanceAppProject.ProfessorLogin.Models;

namespace AttendanceAppProject.ProfessorLogin.Services
{
    public static class MockDataProvider
    {
        public static List<AttendanceRecord> GenerateMockAttendanceData(string professorId, string professorName)
        {
            var records = new List<AttendanceRecord>();
            string[] classes = { "Class A", "Class B", "Class C" };
            string[] questions = {
                "What is the capital of France?",
                "Who wrote Romeo and Juliet?",
                "What is the formula for water?",
                "What is the largest planet in our solar system?",
                "Who painted the Mona Lisa?"
            };
            string[] answers = { "Paris", "Shakespeare", "H2O", "Jupiter", "Da Vinci" };

            var random = new Random();

            for (int i = 1; i <= 30; i++)
            {
                int classIndex = random.Next(classes.Length);
                int questionIndex = random.Next(questions.Length);

                // Create random check-in times over the past 7 days
                DateTime checkInTime = DateTime.Now
                    .AddDays(-random.Next(7))
                    .AddHours(-random.Next(12))
                    .AddMinutes(-random.Next(60));

                // Create student ID with department code and sequential number
                string studentId = $"STU{(100 + i):D3}";

                records.Add(new AttendanceRecord
                {
                    Id = i, // Simulate database ID
                    OrdinalNumber = i,
                    ProfessorId = professorId,
                    ProfessorName = professorName,
                    StudentId = studentId,
                    Class = classes[classIndex],
                    CheckInTime = checkInTime,
                    QuizQuestion = questions[questionIndex],
                    QuizAnswer = answers[questionIndex]
                });
            }

            return records;
        }

        public static ProfessorModel GetMockProfessor(string professorId, string password)
        {
            if (professorId == "js123" && password == "password123")
            {
                return new ProfessorModel
                {
                    ProfessorId = "js123",
                    FullName = "John Smith",
                    Department = "Computer Science",
                    Email = "john.smith@utdallas.edu"
                };
            }
            else if (professorId == "jd123" && password == "password456")
            {
                return new ProfessorModel
                {
                    ProfessorId = "jd123",
                    FullName = "Jane Doe",
                    Department = "Mathematics",
                    Email = "jane.doe@utdallas.edu"
                };
            }
            else if (professorId == "rj123" && password == "password789")
            {
                return new ProfessorModel
                {
                    ProfessorId = "rj123",
                    FullName = "Robert Johnson",
                    Department = "Physics",
                    Email = "robert.johnson@utdallas.edu"
                };
            }
            else if (professorId == "test" && password == "test")
            {
                return new ProfessorModel
                {
                    ProfessorId = "test",
                    FullName = "Test Professor",
                    Department = "Testing Department",
                    Email = "test@example.com"
                };
            }

            return null;
        }
    }
}