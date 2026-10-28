
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

    public async Task AddAsync(AuditLog auditLog)
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

        await _auditLogRepository.AddAsync(auditLog);
    }

    public async Task<List<AuditLog>> GetAllAsync()
    {
        return await _auditLogRepository.GetAllAsync();
    }
}
