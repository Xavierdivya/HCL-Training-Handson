
using CourseManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Learner> Learners => Set<Learner>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(c => c.CourseCode);

            entity.Property(c => c.CourseCode)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(c => c.CourseTitle)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(c => c.Fee)
                .HasPrecision(10, 2);

            entity.ToTable("Courses", table =>
            {
                table.HasCheckConstraint(
                    "CK_Courses_Fee", "[Fee] >= 0");

                table.HasCheckConstraint(
                    "CK_Courses_TotalSeats", "[TotalSeats] > 0");

                table.HasCheckConstraint(
                    "CK_Courses_EnrolledCount",
                    "[EnrolledCount] >= 0 AND [EnrolledCount] <= [TotalSeats]");

                table.HasCheckConstraint(
                    "CK_Courses_DurationHours", "[DurationHours] > 0");
            });
        });

        modelBuilder.Entity<Learner>(entity =>
        {
            entity.HasKey(l => l.LearnerId);

            entity.Property(l => l.LearnerId)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(l => l.LearnerName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(l => l.Email)
                .HasMaxLength(150)
                .IsRequired();

            entity.HasIndex(l => l.Email)
                .IsUnique();
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId);

            entity.Property(e => e.EnrollmentId)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.LearnerId)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.CourseCode)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.EnrollmentDate)
                .IsRequired();

            entity.Property(e => e.BaseFee)
    .HasPrecision(10, 2);

            entity.Property(e => e.DiscountAmount)
                .HasPrecision(10, 2);

            entity.Property(e => e.FinalFee)
                .HasPrecision(10, 2);

            entity.Property(e => e.Status)
                .HasConversion<int>();

            entity.HasOne(e => e.Learner)
                .WithMany(l => l.Enrollments)
                .HasForeignKey(e => e.LearnerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseCode)
                .OnDelete(DeleteBehavior.Restrict);

            entity.ToTable("Enrollments", table =>
            {
                table.HasCheckConstraint(
                    "CK_Enrollments_Fees",
                    "[BaseFee] >= 0 AND [DiscountAmount] >= 0 AND [FinalFee] >= 0");

                table.HasCheckConstraint(
                    "CK_Enrollments_Status",
                    "[Status] IN (0, 1)");
            });
        });
    }
}
