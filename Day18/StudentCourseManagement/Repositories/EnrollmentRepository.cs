
using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;
using StudentCourseManagement.Services;
using System.Data;

namespace StudentCourseManagement.Repositories;

public class EnrollmentRepository
{


    public async Task<int> AddAsync(Enrollment enrollment)
    {
        // Step 1: Check that the student exists and is active.
        StudentRepository studentRepository = new();

        bool studentIsActive =
            await studentRepository.IsStudentActiveAsync(
                enrollment.StudentId);

        if (!studentIsActive)
        {
            throw new InvalidOperationException(
                "Student does not exist or is inactive.");
        }

        // Step 2: Check that the course exists and is active.
        CourseRepository courseRepository = new();

        bool courseIsActive =
            await courseRepository.IsCourseActiveAsync(
                enrollment.CourseId);

        if (!courseIsActive)
        {
            throw new InvalidOperationException(
                "Course does not exist or is inactive.");
        }

        // Step 3: Check the student's active enrollment count.
        int activeEnrollmentCount =
            await GetActiveEnrollmentCountAsync(
                enrollment.StudentId);

        if (activeEnrollmentCount >= 5)
        {
            throw new InvalidOperationException(
                "Student cannot have more than five active enrollments.");
        }

        // Step 4: Insert the enrollment after all validations pass.
        const string sql = """
        INSERT INTO Enrollments (StudentId, CourseId, Status)
        VALUES (@StudentId, @CourseId, @Status);

        SELECT CAST(SCOPE_IDENTITY() AS INT);
        """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@StudentId", SqlDbType.Int)
            .Value = enrollment.StudentId;

        command.Parameters.Add("@CourseId", SqlDbType.Int)
            .Value = enrollment.CourseId;

        command.Parameters.Add("@Status", SqlDbType.NVarChar, 20)
            .Value = enrollment.Status;

        await connection.OpenAsync();

        return Convert.ToInt32(
            await command.ExecuteScalarAsync());
    }


    public async Task<int> AddWithAuditAsync(
        Enrollment enrollment,
        AuditLogService auditLogService)
    {
        // Step 1: Validate the student.
        StudentRepository studentRepository = new();

        bool studentIsActive =
            await studentRepository.IsStudentActiveAsync(
                enrollment.StudentId);

        if (!studentIsActive)
        {
            throw new InvalidOperationException(
                "Student does not exist or is inactive.");
        }

        // Step 2: Validate the course.
        CourseRepository courseRepository = new();

        bool courseIsActive =
            await courseRepository.IsCourseActiveAsync(
                enrollment.CourseId);

        if (!courseIsActive)
        {
            throw new InvalidOperationException(
                "Course does not exist or is inactive.");
        }

        // Step 3: Check the active enrollment limit.
        int activeEnrollmentCount =
            await GetActiveEnrollmentCountAsync(
                enrollment.StudentId);

        if (activeEnrollmentCount >= 5)
        {
            throw new InvalidOperationException(
                "Student cannot have more than five active enrollments.");
        }

        // Step 4: Open one connection and begin one transaction.
        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await connection.OpenAsync();

        await using SqlTransaction transaction =
            (SqlTransaction)await connection.BeginTransactionAsync();

        try
        {
            // Step 5: Insert the enrollment.
            const string sql = """
            INSERT INTO Enrollments
                (StudentId, CourseId, Status)
            VALUES
                (@StudentId, @CourseId, @Status);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

            int enrollmentId;

            await using (SqlCommand command =
                new(sql, connection, transaction))
            {
                command.Parameters.Add("@StudentId", SqlDbType.Int)
                    .Value = enrollment.StudentId;

                command.Parameters.Add("@CourseId", SqlDbType.Int)
                    .Value = enrollment.CourseId;

                command.Parameters.Add("@Status", SqlDbType.NVarChar, 20)
                    .Value = enrollment.Status;

                enrollmentId = Convert.ToInt32(
                    await command.ExecuteScalarAsync());
            }

            // Step 6: Insert the audit log in the same transaction.
            AuditLog auditLog = new()
            {
                EntityName = "Enrollment",
                EntityId = enrollmentId,
                Action = "Created",
                Details =
                    $"Student ID {enrollment.StudentId} enrolled " +
                    $"in Course ID {enrollment.CourseId}."
            };

            await auditLogService.AddAsync(
                auditLog, connection, transaction);

            // Step 7: Commit both inserts together.
            await transaction.CommitAsync();

            return enrollmentId;
        }
        catch
        {
            // Roll back both inserts if an operation fails.
            try
            {
                await transaction.RollbackAsync();
            }
            catch
            {
                // Preserve the original exception.
            }

            throw;
        }
    }


    public async Task<List<Enrollment>> GetAllAsync()
    {
        const string sql = """
            SELECT EnrollmentId, StudentId, CourseId,
                   EnrolledAt, Status
            FROM Enrollments
            ORDER BY EnrollmentId;
            """;

        List<Enrollment> enrollments = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            enrollments.Add(new Enrollment
            {
                EnrollmentId = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                CourseId = reader.GetInt32(2),
                EnrolledAt = reader.GetDateTime(3),
                Status = reader.GetString(4)
            });
        }

        return enrollments;
    }


    public async Task<List<Enrollment>> GetByStudentIdAsync(int studentId)
    {
        const string sql = """
        SELECT e.EnrollmentId,
               e.StudentId,
               e.CourseId,
               e.EnrolledAt,
               e.Status
        FROM Enrollments e
        INNER JOIN Students s
            ON e.StudentId = s.StudentId
        INNER JOIN Courses c
            ON e.CourseId = c.CourseId
        WHERE e.StudentId = @StudentId
        ORDER BY e.EnrolledAt DESC;
        """;

        List<Enrollment> enrollments = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@StudentId", SqlDbType.Int)
            .Value = studentId;

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            enrollments.Add(new Enrollment
            {
                EnrollmentId = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                CourseId = reader.GetInt32(2),
                EnrolledAt = reader.GetDateTime(3),
                Status = reader.GetString(4)
            });
        }

        return enrollments;
    }


    public async Task<List<Enrollment>> GetByCourseIdAsync(int courseId)
    {
        const string sql = """
        SELECT e.EnrollmentId,
               e.StudentId,
               e.CourseId,
               e.EnrolledAt,
               e.Status
        FROM Enrollments e
        INNER JOIN Students s
            ON e.StudentId = s.StudentId
        INNER JOIN Courses c
            ON e.CourseId = c.CourseId
        WHERE e.CourseId = @CourseId
        ORDER BY e.EnrolledAt DESC;
        """;

        List<Enrollment> enrollments = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@CourseId", SqlDbType.Int)
            .Value = courseId;

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            enrollments.Add(new Enrollment
            {
                EnrollmentId = reader.GetInt32(0),
                StudentId = reader.GetInt32(1),
                CourseId = reader.GetInt32(2),
                EnrolledAt = reader.GetDateTime(3),
                Status = reader.GetString(4)
            });
        }

        return enrollments;
    }


    public async Task<bool> UpdateStatusAsync(
        int enrollmentId, string status)
    {
        const string sql = """
            UPDATE Enrollments
            SET Status = @Status
            WHERE EnrollmentId = @EnrollmentId;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@EnrollmentId", SqlDbType.Int)
            .Value = enrollmentId;

        command.Parameters.Add("@Status", SqlDbType.NVarChar, 20)
            .Value = status;

        await connection.OpenAsync();

        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int enrollmentId)
    {
        const string sql = """
            DELETE FROM Enrollments
            WHERE EnrollmentId = @EnrollmentId;
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@EnrollmentId", SqlDbType.Int)
            .Value = enrollmentId;

        await connection.OpenAsync();

        return await command.ExecuteNonQueryAsync() > 0;
    }


    public async Task<List<string>> GetStudentEnrollmentReportAsync()
    {
        const string sql = """
        SELECT s.StudentId,
               s.Name,
               c.CourseName,
               e.Status
        FROM Enrollments e
        INNER JOIN Students s
            ON e.StudentId = s.StudentId
        INNER JOIN Courses c
            ON e.CourseId = c.CourseId
        ORDER BY s.Name, c.CourseName;
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
            int studentId = reader.GetInt32(0);
            string studentName = reader.GetString(1);
            string courseName = reader.GetString(2);
            string status = reader.GetString(3);

            report.Add(
                $"Student ID: {studentId} | " +
                $"Name: {studentName} | " +
                $"Course: {courseName} | " +
                $"Status: {status}");
        }

        return report;
    }



    public async Task<int> GetActiveEnrollmentCountAsync(int studentId)
    {
        const string sql = """
        SELECT COUNT(*)
        FROM Enrollments
        WHERE StudentId = @StudentId
          AND Status = @Status;
        """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@StudentId", SqlDbType.Int)
            .Value = studentId;

        command.Parameters.Add("@Status", SqlDbType.NVarChar, 20)
            .Value = "Enrolled";

        await connection.OpenAsync();

        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

}
