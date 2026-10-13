
using VehicleRentalSystem.Models;
using VehicleRentalSystem.Payments;

namespace VehicleRentalSystem.Services;

public class VehicleRentalService
{
    public Vehicle[] Vehicles { get; }
    private readonly List<Rental> rentals = new();

    public VehicleRentalService(Vehicle[] vehicles)
    {
        Vehicles = vehicles;
    }

    public void DisplayAllVehicles()
    {
        foreach (Vehicle vehicle in Vehicles)
        {
            vehicle.DisplayDetails();
        }
    }

    public void SearchAvailableVehicles(string vehicleType)
    {
        bool found = false;

        foreach (Vehicle vehicle in Vehicles)
        {
            if (!vehicle.IsAvailable ||
                !vehicle.VehicleType.Equals(
                    vehicleType, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            vehicle.DisplayDetails();
            found = true;
        }

        if (!found)
            Console.WriteLine("No matching available vehicles found.");
    }

    public void SortVehiclesByRate()
    {
        Array.Sort(Vehicles, (a, b) =>
            a.DailyRentalRate.CompareTo(b.DailyRentalRate));

        Console.WriteLine("Vehicles sorted by daily rental rate.");
        DisplayAllVehicles();
    }

    public static bool TryParseVehicleId(
        string input, out int vehicleId)
    {
        return int.TryParse(input, out vehicleId);
    }

    public Rental? FindRental(int rentalId)
    {
        return rentals.Find(r => r.RentalId == rentalId);
    }

    public Rental? CreateRental(
        int vehicleId, Customer customer, int rentalDays,
        ref int rentalCounter)
    {
        Vehicle? vehicle = Array.Find(Vehicles,
            v => v.VehicleId == vehicleId);

        if (vehicle is null)
        {
            Console.WriteLine("Vehicle ID does not exist.");
            return null;
        }

        if (!vehicle.IsAvailable)
        {
            Console.WriteLine("This vehicle is already rented.");
            return null;
        }

        RentalPeriod period;

        try
        {
            period = new RentalPeriod(rentalDays);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }

        return CreateRental(vehicle, customer, period, ref rentalCounter);
    }

    public Rental? CreateRental(
        Vehicle vehicle, Customer customer,
        RentalPeriod period, ref int rentalCounter)
    {
        if (!vehicle.IsAvailable)
        {
            Console.WriteLine("This vehicle is already rented.");
            return null;
        }

        if (!Vehicles.Contains(vehicle))
        {
            Console.WriteLine("Vehicle does not belong to this service.");
            return null;
        }

        int nextId = rentalCounter + 1;

        Rental rental = vehicle.VehicleType.ToLowerInvariant() switch
        {
            "car" => new CarRental(
                nextId, customer, vehicle, period),

            "bike" => new BikeRental(
                nextId, customer, vehicle, period),

            _ => throw new InvalidOperationException(
                "Unsupported vehicle type.")
        };

        rentalCounter = nextId;
        vehicle.IsAvailable = false;
        rentals.Add(rental);

        Console.WriteLine($"Rental successful. Rental ID: {rental.RentalId}");
        return rental;
    }

    public bool CancelRental(int rentalId)
    {
        Rental? rental = FindRental(rentalId);

        if (rental is null || !rental.IsActive)
        {
            Console.WriteLine("Active rental not found.");
            return false;
        }

        rental.CancelRental();
        Console.WriteLine("Rental cancelled. Vehicle is available again.");
        return true;
    }

    public (decimal Subtotal, decimal Fees, decimal FinalAmount)
        CalculateRentalCharges(Rental rental)
    {
        decimal vehicleCost = rental.CalculateRentalCost();
        decimal rentalFee = rental.CalculateRentalFee();

        decimal subtotal = vehicleCost + rentalFee;
        decimal discount = rental.CalculateDiscount(subtotal);
        decimal taxableAmount = subtotal - discount;
        decimal tax = taxableAmount * 0.05m;
        decimal finalAmount = taxableAmount + tax;

        return (subtotal, tax, finalAmount);
    }

    public bool ReturnVehicle(
        int rentalId, IPaymentProcessor processor, string paymentMethod)
    {
        Rental? rental = FindRental(rentalId);

        if (rental is null || !rental.IsActive)
        {
            Console.WriteLine("Active rental not found.");
            return false;
        }

        var charges = CalculateRentalCharges(rental);
        decimal discount = rental.CalculateDiscount(charges.Subtotal);

        Console.WriteLine("\n========== RENTAL BILL ==========");
        Console.WriteLine($"Rental ID: {rental.RentalId}");
        Console.WriteLine($"Customer: {rental.Customer.Name}");
        Console.WriteLine(
            $"Vehicle: {rental.Vehicle.Brand} {rental.Vehicle.Model}");
        Console.WriteLine($"Rental days: {rental.Period.RentalDays}");
        Console.WriteLine($"Subtotal: ₹{charges.Subtotal:F2}");
        Console.WriteLine($"Discount: ₹{discount:F2}");
        Console.WriteLine($"Tax: ₹{charges.Fees:F2}");
        Console.WriteLine($"Final amount: ₹{charges.FinalAmount:F2}");

        RentalChargeSummary summary = new(
            charges.Subtotal, discount, charges.Fees,
            charges.FinalAmount);

        PrintChargeSummary(in summary);

        bool paymentSuccessful =
            processor.ProcessPayment(charges.FinalAmount);

        if (!paymentSuccessful)
        {
            Console.WriteLine("Payment failed. Rental remains active.");
            return false;
        }

        rental.CompleteReturn();

        RentalSummary receipt = new(
            rental.RentalId,
            rental.Customer.Name,
            rental.Vehicle.VehicleId,
            $"{rental.Vehicle.Brand} {rental.Vehicle.Model}",
            rental.Period.RentalDays,
            charges.Subtotal,
            discount,
            charges.Fees,
            charges.FinalAmount,
            paymentMethod);

        Console.WriteLine("\nVehicle returned successfully.");
        Console.WriteLine($"Receipt created for rental {receipt.RentalId}.");

        RentalSummary copy = receipt with { };
        Console.WriteLine($"Receipt copy equals original: {receipt == copy}");

        return true;
    }

    public bool ReturnVehicle(
        Rental rental, IPaymentProcessor processor, string paymentMethod)
    {
        return ReturnVehicle(rental.RentalId, processor, paymentMethod);
    }

    private void PrintChargeSummary(in RentalChargeSummary summary)
    {
        Console.WriteLine(
            $"Amount due: ₹{summary.FinalAmount:F2}");
    }
    public void DisplayActiveRentals()
    {
        bool found = false;

        foreach (Rental rental in rentals)
        {
            if (!rental.IsActive)
                continue;

            Console.WriteLine(
                $"Rental ID: {rental.RentalId} | " +
                $"Vehicle ID: {rental.Vehicle.VehicleId} | " +
                $"Customer: {rental.Customer.Name}");

            found = true;
        }

        if (!found)
            Console.WriteLine("No active rentals found.");
    }
}
