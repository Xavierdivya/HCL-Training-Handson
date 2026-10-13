
namespace VehicleRentalSystem.Models;

public class Vehicle
{
    public int VehicleId { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public string VehicleType { get; private set; }
    public decimal DailyRentalRate { get; private set; }
    public bool IsAvailable { get; set; }
    public VehicleLocation Location { get; private set; }

    public Vehicle(int vehicleId, string brand, string model,
                   string vehicleType, decimal dailyRentalRate,
                   VehicleLocation location)
    {
        if (dailyRentalRate <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(dailyRentalRate), "Rate must be positive.");

        this.VehicleId = vehicleId;
        this.Brand = brand;
        this.Model = model;
        this.VehicleType = vehicleType;
        this.DailyRentalRate = dailyRentalRate;
        this.Location = location;
        this.IsAvailable = true;
    }

    public void DisplayDetails()
    {
        Console.WriteLine(
            $"ID: {VehicleId} | {Brand} {Model} | " +
            $"Type: {VehicleType} | Daily Rate: ₹{DailyRentalRate:F2} | " +
            $"Available: {IsAvailable} | Location: {Location}");
    }
}
