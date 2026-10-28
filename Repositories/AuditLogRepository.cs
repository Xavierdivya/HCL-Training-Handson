
using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class AuditLogRepository
{
    public async Task AddAsync(AuditLog auditLog)
    {
        const string sql = """
            INSERT INTO AuditLogs
                (EntityName, EntityId, Action, Details)
            VALUES
                (@EntityName, @EntityId, @Action, @Details);
            """;

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        command.Parameters.AddWithValue(
            "@EntityName", auditLog.EntityName);

        command.Parameters.AddWithValue(
            "@EntityId", auditLog.EntityId);

        command.Parameters.AddWithValue(
            "@Action", auditLog.Action);

        command.Parameters.AddWithValue(
            "@Details", (object?)auditLog.Details ?? DBNull.Value);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<AuditLog>> GetAllAsync()
    {
        const string sql = """
            SELECT AuditId, EntityName, EntityId,
                   Action, Details, CreatedAt
            FROM AuditLogs
            ORDER BY AuditId DESC;
            """;

        List<AuditLog> auditLogs = new();

        await using SqlConnection connection =
            DatabaseHelper.CreateConnection();

        await using SqlCommand command = new(sql, connection);

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            auditLogs.Add(new AuditLog
            {
                AuditId = reader.GetInt64(0),
                EntityName = reader.GetString(1),
                EntityId = reader.GetInt32(2),
                Action = reader.GetString(3),
                Details = reader.IsDBNull(4)
                    ? null
                    : reader.GetString(4),
                CreatedAt = reader.GetDateTime(5)
            });
        }

        return auditLogs;
    }
}
