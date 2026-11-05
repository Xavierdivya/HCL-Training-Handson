
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<College> Colleges { get; set; }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ApplicationUser configuration
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.FullName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(u => u.AccountStatus)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.HasIndex(u => u.CollegeId);
            });

            // College configuration
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

                // One College can have multiple representative accounts.
                // Existing users and SuperAdmin can have a null CollegeId.
                entity.HasMany(c => c.Representatives)
                    .WithOne(u => u.College)
                    .HasForeignKey(u => u.CollegeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
