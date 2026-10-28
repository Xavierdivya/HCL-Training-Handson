
using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;

namespace StudentCourseManagement.Repositories;

public class ReportRepository
{
    public async Task<List<string>> GetEnrollmentReportAsync()
    {
        const string sql = """
            SELECT
                e.EnrollmentId,
                s.Name AS StudentName,
                c.CourseName,
                d.DepartmentName,
                e.EnrolledAt,
                e.Status
            FROM Enrollments e
            INNER JOIN Students s
                ON e.StudentId = s.StudentId
            INNER JOIN Courses c
                ON e.CourseId = c.CourseId
            INNER JOIN Departments d
                ON c.DepartmentId = d.DepartmentId
            ORDER BY e.EnrollmentId;
            """;

        List<string> report = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            report.Add(
                $"Enrollment ID: {reader.GetInt32(0)} | " +
                $"Student: {reader.GetString(1)} | " +
                $"Course: {reader.GetString(2)} | " +
                $"Department: {reader.GetString(3)} | " +
                $"Date: {reader.GetDateTime(4):g} | " +
                $"Status: {reader.GetString(5)}");
        }

        return report;
    }
}
