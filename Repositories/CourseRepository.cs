
using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;
using System.Data;

namespace StudentCourseManagement.Repositories;

public class CourseRepository
{
    public async Task<int> AddAsync(Course course)
    {
        const string sql = """
            INSERT INTO Courses
                (CourseName, DurationMonths, Fee, DepartmentId)
            VALUES
                (@CourseName, @DurationMonths, @Fee, @DepartmentId);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();
        await using SqlCommand command = new(sql, connection);

        AddParameters(command, course);

        await connection.OpenAsync();
        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public async Task<List<Course>> GetAllAsync()
    {
        const string sql = """
            SELECT CourseId, CourseName, DurationMonths,
                   Fee, DepartmentId, IsActive
            FROM Courses
            ORDER BY CourseId;
            """;

        List<Course> courses = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();
        await using SqlCommand command = new(sql, connection);

        await connection.OpenAsync();
        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            courses.Add(new Course
            {
                CourseId = reader.GetInt32(0),
                CourseName = reader.GetString(1),
                DurationMonths = reader.GetInt32(2),
                Fee = reader.GetDecimal(3),
                DepartmentId = reader.GetInt32(4),
                IsActive = reader.GetBoolean(5)
            });
        }

        return courses;
    }

    public async Task<bool> UpdateAsync(Course course)
    {
        const string sql = """
            UPDATE Courses
            SET CourseName = @CourseName,
                DurationMonths = @DurationMonths,
                Fee = @Fee,
                DepartmentId = @DepartmentId
            WHERE CourseId = @CourseId;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();
        await using SqlCommand command = new(sql, connection);

        AddParameters(command, course);
        command.Parameters.Add("@CourseId", SqlDbType.Int)
            .Value = course.CourseId;

        await connection.OpenAsync();
        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int courseId)
    {
        const string sql = """
            DELETE FROM Courses
            WHERE CourseId = @CourseId;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();
        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@CourseId", SqlDbType.Int)
            .Value = courseId;

        await connection.OpenAsync();
        return await command.ExecuteNonQueryAsync() > 0;
    }

    private static void AddParameters(
        SqlCommand command, Course course)
    {
        command.Parameters.Add(
            "@CourseName", SqlDbType.NVarChar, 100)
            .Value = course.CourseName;

        command.Parameters.Add(
            "@DurationMonths", SqlDbType.Int)
            .Value = course.DurationMonths;

        SqlParameter feeParameter =
            command.Parameters.Add("@Fee", SqlDbType.Decimal);

        feeParameter.Precision = 10;
        feeParameter.Scale = 2;
        feeParameter.Value = course.Fee;

        command.Parameters.Add("@DepartmentId", SqlDbType.Int)
            .Value = course.DepartmentId;
    }
}
