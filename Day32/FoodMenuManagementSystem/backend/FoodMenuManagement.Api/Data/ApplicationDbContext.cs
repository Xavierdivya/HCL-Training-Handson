
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
    }
}
