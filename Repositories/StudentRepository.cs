
using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;
using System.Data;

namespace StudentCourseManagement.Repositories;

public class StudentRepository
{
    public async Task<int> AddAsync(Student student)
    {
        const string sql = """
            INSERT INTO Students
                (Name, Email, Age, DepartmentId)
            VALUES
                (@Name, @Email, @Age, @DepartmentId);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add(
            "@Name", SqlDbType.NVarChar, 100)
            .Value = student.Name;

        command.Parameters.Add(
            "@Email", SqlDbType.NVarChar, 255)
            .Value = student.Email;

        command.Parameters.Add(
            "@Age", SqlDbType.Int)
            .Value = student.Age;

        command.Parameters.Add(
            "@DepartmentId", SqlDbType.Int)
            .Value = student.DepartmentId;

        await connection.OpenAsync();

        object? result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    public async Task<List<Student>> GetAllAsync()
    {
        const string sql = """
            SELECT StudentId, Name, Email, Age, DepartmentId,
                   AdmissionDate, IsActive
            FROM Students
            ORDER BY StudentId;
            """;

        List<Student> students = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Student student = new()
            {
                StudentId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Age = reader.GetInt32(3),
                DepartmentId = reader.GetInt32(4),
                AdmissionDate = reader.GetDateTime(5),
                IsActive = reader.GetBoolean(6)
            };

            students.Add(student);
        }

        return students;
    }


    public async Task<List<Student>> SearchByNameAsync(string name)
    {
        const string sql = """
        SELECT StudentId, Name, Email, Age, DepartmentId,
               AdmissionDate, IsActive
        FROM Students
        WHERE Name LIKE @Name
        ORDER BY StudentId;
        """;

        List<Student> students = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@Name", SqlDbType.NVarChar, 100)
            .Value = $"%{name}%";

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Student student = new()
            {
                StudentId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Age = reader.GetInt32(3),
                DepartmentId = reader.GetInt32(4),
                AdmissionDate = reader.GetDateTime(5),
                IsActive = reader.GetBoolean(6)
            };

            students.Add(student);
        }

        return students;
    }


    public async Task<bool> UpdateAsync(Student student)
    {
        const string sql = """
        UPDATE Students
        SET Name = @Name,
            Email = @Email,
            Age = @Age,
            DepartmentId = @DepartmentId
        WHERE StudentId = @StudentId;
        """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@StudentId", SqlDbType.Int)
            .Value = student.StudentId;

        command.Parameters.Add("@Name", SqlDbType.NVarChar, 100)
            .Value = student.Name;

        command.Parameters.Add("@Email", SqlDbType.NVarChar, 255)
            .Value = student.Email;

        command.Parameters.Add("@Age", SqlDbType.Int)
            .Value = student.Age;

        command.Parameters.Add("@DepartmentId", SqlDbType.Int)
            .Value = student.DepartmentId;

        await connection.OpenAsync();

        return await command.ExecuteNonQueryAsync() > 0;
    }


    public async Task<bool> DeleteAsync(int studentId)
    {
        const string sql = """
        UPDATE Students
        SET IsActive = 0
        WHERE StudentId = @StudentId
          AND IsActive = 1;
        """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@StudentId", SqlDbType.Int)
            .Value = studentId;

        await connection.OpenAsync();

        return await command.ExecuteNonQueryAsync() > 0;
    }


    public async Task<bool> IsStudentActiveAsync(int studentId)
    {
        const string sql = """
        SELECT COUNT(1)
        FROM Students
        WHERE StudentId = @StudentId
          AND IsActive = 1;
        """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@StudentId", SqlDbType.Int).Value =
            studentId;

        await connection.OpenAsync();

        int count = Convert.ToInt32(
            await command.ExecuteScalarAsync());

        return count > 0;
    }


}
