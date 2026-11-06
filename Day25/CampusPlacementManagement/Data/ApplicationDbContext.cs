
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

        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyHR> CompanyHRs { get; set; }

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

                entity.HasIndex(u => u.CompanyId);
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

                entity.HasOne(p => p.User)
                    .WithOne()
                    .HasForeignKey<PlacementOfficer>(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.College)
                    .WithMany()
                    .HasForeignKey(p => p.CollegeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Company configuration
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

                // One company can have multiple Company HR accounts.
                entity.HasMany(c => c.HRAccounts)
                    .WithOne(u => u.Company)
                    .HasForeignKey(u => u.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // CompanyHR configuration
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

                // One Identity user has one CompanyHR profile.
                entity.HasOne(h => h.User)
                    .WithOne()
                    .HasForeignKey<CompanyHR>(h => h.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // One company can have multiple CompanyHR profiles.
                entity.HasOne(h => h.Company)
                    .WithMany()
                    .HasForeignKey(h => h.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
