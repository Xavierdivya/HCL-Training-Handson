
using Microsoft.EntityFrameworkCore;
using FoodMenuManagement.Api.Entities;

namespace FoodMenuManagement.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<FoodItem> FoodItems { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
    .HasKey(u => u.UserId);

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<AppUser>()
                .Property(u => u.Email)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<AppUser>()
                .Property(u => u.FullName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<AppUser>()
                .Property(u => u.Role)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}
