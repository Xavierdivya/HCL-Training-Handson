
using Microsoft.Data.SqlClient;
using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class AuditLogService
{
    private readonly AuditLogRepository _auditLogRepository;

    public AuditLogService(AuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    // Normal audit logging using its own database connection.
    public async Task AddAsync(AuditLog auditLog)
    {
        ValidateAuditLog(auditLog);

        await _auditLogRepository.AddAsync(auditLog);
    }

    // Transaction-based audit logging using an existing connection.
    public async Task AddAsync(
        AuditLog auditLog,
        SqlConnection connection,
        SqlTransaction transaction)
    {
        ValidateAuditLog(auditLog);

        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(transaction);

        await _auditLogRepository.AddAsync(
            auditLog, connection, transaction);
    }

    // Shared validation for both methods.
    private static void ValidateAuditLog(AuditLog auditLog)
    {
        ArgumentNullException.ThrowIfNull(auditLog);

        if (string.IsNullOrWhiteSpace(auditLog.EntityName))
        {
            throw new ArgumentException(
                "Entity name is required.");
        }

        if (auditLog.EntityId <= 0)
        {
            throw new ArgumentException(
                "Entity ID must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(auditLog.Action))
        {
            throw new ArgumentException(
                "Action is required.");
        }

        if (auditLog.EntityName.Length > 50)
        {
            throw new ArgumentException(
                "Entity name cannot exceed 50 characters.");
        }

        if (auditLog.Action.Length > 20)
        {
            throw new ArgumentException(
                "Action cannot exceed 20 characters.");
        }

        if (auditLog.Details?.Length > 500)
        {
            throw new ArgumentException(
                "Details cannot exceed 500 characters.");
        }
    }

    public async Task<List<AuditLog>> GetAllAsync()
    {
        return await _auditLogRepository.GetAllAsync();
    }
}
