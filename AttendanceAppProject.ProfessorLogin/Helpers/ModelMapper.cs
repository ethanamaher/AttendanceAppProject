// Canh Nguyen

using System.Collections.Generic;
using System.Linq;
using AttendanceAppProject.ProfessorLogin.Models;
using ApiModels = AttendanceAppProject.ApiService.Models;

namespace AttendanceAppProject.ProfessorLogin.Helpers
{
    public static class ModelMapper
    {
        // Convert API professor model to UI professor model
        public static ProfessorModel ToProfessorModel(ApiModels.Professor professor)
        {
            if (professor == null) return null;

            return new ProfessorModel
            {
                ProfessorId = professor.ProfessorId,
                FullName = professor.FullName,
                Department = professor.Department,
                Email = professor.Email
            };
        }

        // Convert API attendance record to UI attendance record - use fully qualified names
        public static AttendanceAppProject.ProfessorLogin.Models.AttendanceRecord ToAttendanceRecord(
            ApiModels.AttendanceRecord record)
        {
            if (record == null) return null;

            return new AttendanceAppProject.ProfessorLogin.Models.AttendanceRecord
            {
                Id = record.Id,
                OrdinalNumber = record.OrdinalNumber,
                ProfessorId = record.ProfessorId,
                ProfessorName = record.ProfessorName,
                StudentId = record.StudentId,
                Class = record.Class,
                CheckInTime = record.CheckInTime,
                QuizQuestion = record.QuizQuestion,
                QuizAnswer = record.QuizAnswer
            };
        }

        // Convert list of API attendance records to list of UI attendance records
        public static List<AttendanceAppProject.ProfessorLogin.Models.AttendanceRecord> ToAttendanceRecords(
            IEnumerable<ApiModels.AttendanceRecord> records)
        {
            if (records == null) return new List<AttendanceAppProject.ProfessorLogin.Models.AttendanceRecord>();

            return records.Select(r => ToAttendanceRecord(r)).ToList();
        }
    }
}