using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<College> Colleges { get; set; }

        public DbSet<PlacementOfficer> PlacementOfficers { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyHR> CompanyHRs { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<PlacementDrive> PlacementDrives { get; set; }

        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<InterviewSchedule> InterviewSchedules { get; set; }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.FullName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(u => u.AccountStatus)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.HasIndex(u => u.CollegeId);
                entity.HasIndex(u => u.CompanyId);
            });

            builder.Entity<College>(entity =>
            {
                entity.HasKey(c => c.CollegeId);

                entity.Property(c => c.CollegeName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(c => c.CollegeCode)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.HasIndex(c => c.CollegeCode)
                    .IsUnique();

                entity.Property(c => c.UniversityName)
                    .HasMaxLength(150);

                entity.Property(c => c.OfficialEmail)
                    .HasMaxLength(256)
                    .IsRequired();

                entity.Property(c => c.PhoneNumber)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(c => c.Address)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(c => c.Website)
                    .HasMaxLength(250);

                entity.Property(c => c.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(c => c.CreatedAt)
                    .IsRequired();

                entity.HasMany(c => c.Representatives)
                    .WithOne(u => u.College)
                    .HasForeignKey(u => u.CollegeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<PlacementOfficer>(entity =>
            {
                entity.HasKey(p => p.PlacementOfficerId);

                entity.Property(p => p.UserId)
                    .IsRequired();

                entity.HasIndex(p => p.UserId)
                    .IsUnique();

                entity.Property(p => p.Designation)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(p => p.CreatedAt)
                    .IsRequired();

                entity.HasOne(p => p.User)
                    .WithOne()
                    .HasForeignKey<PlacementOfficer>(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.College)
                    .WithMany()
                    .HasForeignKey(p => p.CollegeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Company>(entity =>
            {
                entity.HasKey(c => c.CompanyId);

                entity.Property(c => c.CompanyName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(c => c.CompanyCode)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.HasIndex(c => c.CompanyCode)
                    .IsUnique();

                entity.Property(c => c.Industry)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(c => c.OfficialEmail)
                    .HasMaxLength(256)
                    .IsRequired();

                entity.Property(c => c.PhoneNumber)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(c => c.Address)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(c => c.Website)
                    .HasMaxLength(250);

                entity.Property(c => c.Description)
                    .HasMaxLength(1000);

                entity.Property(c => c.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(c => c.CreatedAt)
                    .IsRequired();

                entity.HasMany(c => c.HRAccounts)
                    .WithOne(u => u.Company)
                    .HasForeignKey(u => u.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<CompanyHR>(entity =>
            {
                entity.HasKey(h => h.CompanyHRId);

                entity.Property(h => h.UserId)
                    .IsRequired();

                entity.HasIndex(h => h.UserId)
                    .IsUnique();

                entity.Property(h => h.Designation)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(h => h.CreatedAt)
                    .IsRequired();

                entity.HasOne(h => h.User)
                    .WithOne()
                    .HasForeignKey<CompanyHR>(h => h.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(h => h.Company)
                    .WithMany()
                    .HasForeignKey(h => h.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.StudentId);

                entity.Property(s => s.UserId)
                    .IsRequired();

                entity.HasIndex(s => s.UserId)
                    .IsUnique();

                entity.Property(s => s.RegisterNumber)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.HasIndex(s => new
                {
                    s.CollegeId,
                    s.RegisterNumber
                }).IsUnique();

                entity.Property(s => s.Department)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(s => s.Degree)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(s => s.CGPA)
                    .HasPrecision(4, 2);

                entity.Property(s => s.ActiveBacklogs)
                    .IsRequired();

                entity.Property(s => s.ResumeUrl)
                    .HasMaxLength(500);

                entity.Property(s => s.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(s => s.CreatedAt)
                    .IsRequired();

                entity.HasOne(s => s.User)
                    .WithOne()
                    .HasForeignKey<Student>(s => s.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.College)
                    .WithMany()
                    .HasForeignKey(s => s.CollegeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.PlacementOfficer)
                    .WithMany()
                    .HasForeignKey(s => s.PlacementOfficerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Job>(entity =>
            {
                entity.HasKey(j => j.JobId);

                entity.Property(j => j.JobTitle)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(j => j.JobDescription)
                    .HasMaxLength(3000)
                    .IsRequired();

                entity.Property(j => j.Location)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(j => j.EmploymentType)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(j => j.MinimumCGPA)
                    .HasPrecision(4, 2);

                entity.Property(j => j.MaximumActiveBacklogs)
                    .IsRequired();

                entity.Property(j => j.EligibleDepartments)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(j => j.EligibleDegrees)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(j => j.GraduationYearFrom)
                    .IsRequired();

                entity.Property(j => j.GraduationYearTo)
                    .IsRequired();

                entity.Property(j => j.ApplicationDeadline)
                    .IsRequired();

                entity.Property(j => j.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(j => j.CreatedAt)
                    .IsRequired();

                entity.HasOne(j => j.Company)
                    .WithMany()
                    .HasForeignKey(j => j.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<PlacementDrive>(entity =>
            {
                entity.HasKey(pd => pd.PlacementDriveId);

                entity.Property(pd => pd.DriveName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(pd => pd.Description)
                    .IsRequired()
                    .HasMaxLength(3000);

                entity.Property(pd => pd.Venue)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(pd => pd.MinimumCGPA)
                    .HasPrecision(4, 2);

                entity.Property(pd => pd.MaximumActiveBacklogs)
                    .IsRequired();

                entity.Property(pd => pd.EligibleDepartments)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(pd => pd.EligibleDegrees)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(pd => pd.GraduationYearFrom)
                    .IsRequired();

                entity.Property(pd => pd.GraduationYearTo)
                    .IsRequired();

                entity.Property(pd => pd.Status)
                    .HasConversion<string>()
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(pd => pd.CreatedAt)
                    .IsRequired();

                entity.HasOne(pd => pd.College)
                    .WithMany()
                    .HasForeignKey(pd => pd.CollegeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(pd => pd.Company)
                    .WithMany()
                    .HasForeignKey(pd => pd.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(pd => pd.PlacementOfficer)
                    .WithMany()
                    .HasForeignKey(pd => pd.PlacementOfficerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(pd => pd.CollegeId);
                entity.HasIndex(pd => pd.CompanyId);
                entity.HasIndex(pd => pd.PlacementOfficerId);
            });

            builder.Entity<JobApplication>(entity =>
            {
                entity.HasKey(ja => ja.JobApplicationId);

                entity.Property(ja => ja.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(ja => ja.AppliedAt)
                    .IsRequired();

                entity.Property(ja => ja.CoverLetter)
                    .HasMaxLength(2000);

                entity.Property(ja => ja.Remarks)
                    .HasMaxLength(1000);

                entity.HasIndex(ja => new
                {
                    ja.JobId,
                    ja.StudentId
                }).IsUnique();

                entity.HasOne(ja => ja.Job)
                    .WithMany()
                    .HasForeignKey(ja => ja.JobId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ja => ja.Student)
                    .WithMany()
                    .HasForeignKey(ja => ja.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Interview Schedule configuration.
            builder.Entity<InterviewSchedule>(entity =>
            {
                entity.HasKey(i => i.InterviewScheduleId);

                entity.Property(i => i.ScheduledAt)
                    .IsRequired();

                entity.Property(i => i.DurationMinutes)
                    .IsRequired();

                entity.Property(i => i.Mode)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(i => i.MeetingDetails)
                    .HasMaxLength(1000)
                    .IsRequired();

                entity.Property(i => i.Notes)
                    .HasMaxLength(2000);

                entity.Property(i => i.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(i => i.Feedback)
                    .HasMaxLength(2000);

                entity.Property(i => i.CreatedAt)
                    .IsRequired();

                entity.HasOne(i => i.JobApplication)
                    .WithMany()
                    .HasForeignKey(i => i.JobApplicationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.Company)
                    .WithMany()
                    .HasForeignKey(i => i.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.CompanyHR)
                    .WithMany()
                    .HasForeignKey(i => i.CompanyHRId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(i => i.JobApplicationId);
                entity.HasIndex(i => i.CompanyId);
                entity.HasIndex(i => i.CompanyHRId);
                entity.HasIndex(i => i.ScheduledAt);
            });
        }
    }
}