
using LeaveManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

var connectionString =
    "Server=localhost\\SQLEXPRESS;Database=LeaveManagementDb;Trusted_Connection=True;TrustServerCertificate=True;";

var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseSqlServer(connectionString)
    .Options;

using var dbContext = new ApplicationDbContext(options);

Console.WriteLine("Leave Management System");
Console.WriteLine("Database context configured successfully.");
