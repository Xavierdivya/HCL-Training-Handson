
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<College> Colleges { get; set; }

        public DbSet<PlacementOfficer> PlacementOfficers { get; set; }

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

                // One college can have multiple representative accounts.
                entity.HasMany(c => c.Representatives)
                    .WithOne(u => u.College)
                    .HasForeignKey(u => u.CollegeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // PlacementOfficer configuration
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

                // One Identity user can have one PlacementOfficer profile.
                entity.HasOne(p => p.User)
                    .WithOne()
                    .HasForeignKey<PlacementOfficer>(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // One college can have multiple PlacementOfficer profiles.
                entity.HasOne(p => p.College)
                    .WithMany()
                    .HasForeignKey(p => p.CollegeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
