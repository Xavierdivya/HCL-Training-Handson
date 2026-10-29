
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LeaveManagementSystem.Tests
{
    public class LeaveRequestServiceTests
    {
        private async Task<(ApplicationDbContext Context,
            LeaveRequestService Service)> CreateServiceAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);

            context.Employees.Add(new Employee
            {
                EmployeeId = 1,
                FullName = "Test Employee",
                Email = "test@example.com",
                IsActive = true
            });

            context.LeaveTypes.Add(new LeaveType
            {
                LeaveTypeId = 1,
                LeaveTypeName = "Casual Leave",
                MaximumDays = 5,
                IsActive = true
            });

            await context.SaveChangesAsync();

            return (context, new LeaveRequestService(context));
        }

        [Fact]
        public async Task SubmitLeaveRequest_WithValidData_Succeeds()
        {
            var (context, service) = await CreateServiceAsync();

            try
            {
                var result = await service.AddLeaveRequestAsync(
                    1, 1,
                    new DateOnly(2026, 11, 10),
                    new DateOnly(2026, 11, 12),
                    "Family function");

                Assert.True(result.Success);

                var savedRequest = await context.LeaveRequests
                    .SingleAsync();

                Assert.Equal(LeaveStatus.Pending, savedRequest.Status);
                Assert.Equal("Family function", savedRequest.Reason);
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        [Fact]
        public async Task SubmitLeaveRequest_WhenEndDateIsBeforeStartDate_Fails()
        {
            var (context, service) = await CreateServiceAsync();

            try
            {
                var result = await service.AddLeaveRequestAsync(
                    1, 1,
                    new DateOnly(2026, 11, 15),
                    new DateOnly(2026, 11, 10),
                    "Family function");

                Assert.False(result.Success);
            }
            finally
            {
                await context.DisposeAsync();
            }
        }

        [Fact]
        public async Task SubmitLeaveRequest_WhenDatesOverlapPendingRequest_Fails()
        {
            var (context, service) = await CreateServiceAsync();

            try
            {
                context.LeaveRequests.Add(new LeaveRequest
                {
                    EmployeeId = 1,
                    LeaveTypeId = 1,
                    StartDate = new DateOnly(2026, 11, 10),
                    EndDate = new DateOnly(2026, 11, 12),
                    Reason = "Existing request",
                    Status = LeaveStatus.Pending
                });

                await context.SaveChangesAsync();

                var result = await service.AddLeaveRequestAsync(
                    1, 1,
                    new DateOnly(2026, 11, 12),
                    new DateOnly(2026, 11, 14),
                    "Another request");

                Assert.False(result.Success);
                Assert.Contains("overlaps", result.Message);
            }
            finally
            {
                await context.DisposeAsync();
            }
        }
        
[Fact]
public async Task SubmitLeaveRequest_WhenDatesOverlapApprovedRequest_Fails()
{
    var (context, service) = await CreateServiceAsync();

    try
    {
        context.LeaveRequests.Add(new LeaveRequest
        {
            EmployeeId = 1,
            LeaveTypeId = 1,
            StartDate = new DateOnly(2026, 11, 10),
            EndDate = new DateOnly(2026, 11, 12),
            Reason = "Approved leave",
            Status = LeaveStatus.Approved
        });

        await context.SaveChangesAsync();

        var result = await service.AddLeaveRequestAsync(
            1, 1,
            new DateOnly(2026, 11, 11),
            new DateOnly(2026, 11, 13),
            "Another request");

        Assert.False(result.Success);
        Assert.Contains("overlaps", result.Message);
    }
    finally
    {
        await context.DisposeAsync();
    }
}

[Fact]
public async Task SubmitLeaveRequest_WhenLeaveExceedsMaximumDays_Fails()
{
    var (context, service) = await CreateServiceAsync();

    try
    {
        var result = await service.AddLeaveRequestAsync(
            1, 1,
            new DateOnly(2026, 11, 10),
            new DateOnly(2026, 11, 16),
            "Long vacation");

        Assert.False(result.Success);
        Assert.Contains("7 calendar days", result.Message);
    }
    finally
    {
        await context.DisposeAsync();
    }
}

    }

}
