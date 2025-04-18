// Canh Nguyen

using System;
using System.Collections.Generic;
using AttendanceAppProject.Dto.Models;

namespace AttendanceAppProject.ProfessorLogin.Services
{
    public static class MockDataProvider
    {
        public static List<AttendanceInstanceDto> GenerateMockAttendanceData(string professorId, string professorName)
        {
            var records = new List<AttendanceInstanceDto>();
            string[] classes = { "CS 1337: Intro to Programming", "CS 3354: Software Engineering", "CS 4485: Senior Design" };
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

                records.Add(new AttendanceInstanceDto
                {
                    StudentId = studentId,
                    ClassId = Guid.NewGuid(), // Random placeholder
                    IpAddress = "127.0.0.1",
                    IsLate = random.NextDouble() > 0.8,
                    ExcusedAbsence = random.NextDouble() > 0.9,
                    DateTime = checkInTime
                });
            }

            return records;
        }

        public static ProfessorDto GetMockProfessor(string professorId, string password)
        {
            if (professorId == "js123" && password == "password123")
            {
                return new ProfessorDto
                {
                    UtdId = "js123",
                    FirstName = "John",
                    LastName = "Smith",
                    Password = "password123"
                };
            }
            else if (professorId == "jd123" && password == "password456")
            {
                return new ProfessorDto
                {
                    UtdId = "jd123",
                    FirstName = "Jane",
                    LastName = "Doe",
                    Password = "password456"
                };
            }
            else if (professorId == "rj123" && password == "password789")
            {
                return new ProfessorDto
                {
                    UtdId = "rj123",
                    FirstName = "Robert",
                    LastName = "Johnson",
                    Password = "password789"
                };
            }
            else if (professorId == "test" && password == "test")
            {
                return new ProfessorDto
                {
                    UtdId = "test",
                    FirstName = "Test",
                    LastName = "Professor",
                    Password = "test"
                };
            }

            return null;
        }
    }
}