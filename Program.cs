
using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;

try
{
    await using SqlConnection connection =
        DatabaseHelper.CreateConnection();

    await connection.OpenAsync();

    Console.WriteLine("Database connection successful!");
    Console.WriteLine($"Database: {connection.Database}");
}
catch (SqlException ex)
{
    Console.WriteLine("Database connection failed.");
    Console.WriteLine(ex.Message);
}
