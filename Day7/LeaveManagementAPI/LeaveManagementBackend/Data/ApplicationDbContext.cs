
using LeaveManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LeaveRequest> LeaveRequests =>
            Set<LeaveRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LeaveRequest>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.Property(l => l.EmployeeName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(l => l.LeaveType)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(l => l.Reason)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(l => l.Status)
                    .HasMaxLength(20)
                    .IsRequired();
            });
        }
    }
}
