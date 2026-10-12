
using BankingAccountAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingAccountAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BankAccount> BankAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankAccount>()
                .HasIndex(a => a.AccountNumber)
                .IsUnique();

            modelBuilder.Entity<BankAccount>()
                .Property(a => a.Balance)
                .HasPrecision(14, 2);

            modelBuilder.Entity<BankAccount>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_BankAccounts_Balance_NonNegative",
                    "[Balance] >= 0"));
        }
    }
}
