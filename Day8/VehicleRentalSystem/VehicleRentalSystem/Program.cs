
using VehicleRentalSystem.Models;
using VehicleRentalSystem.Payments;
using VehicleRentalSystem.Services;

namespace VehicleRentalSystem;

internal class Program
{
    static void Main()
    {
        decimal GetRate(string vehicleType) =>
            vehicleType.ToLowerInvariant() switch
            {
                "car" => 2500m,
                "bike" => 800m,
                _ => 0m
            };

        Vehicle[] vehicles =
        {
            new Vehicle(101, "Toyota", "Innova", "Car",
                GetRate("Car"), new VehicleLocation("Chennai", "A1")),

            new Vehicle(102, "Hyundai", "i20", "Car",
                GetRate("Car"), new VehicleLocation("Chennai", "A2")),

            new Vehicle(201, "Honda", "Activa", "Bike",
                GetRate("Bike"), new VehicleLocation("Chennai", "B1")),

            new Vehicle(202, "TVS", "Apache", "Bike",
                GetRate("Bike"), new VehicleLocation("Chennai", "B2"))
        };

        VehicleRentalService service = new(vehicles);

        List<Customer> customers = new();
        int customerCounter = 0;
        int rentalCounter = 1000;

        while (true)
        {
            Console.WriteLine("\n===== VEHICLE RENTAL SYSTEM =====");
            Console.WriteLine("1. Display all vehicles");
            Console.WriteLine("2. Search available vehicles");
            Console.WriteLine("3. Sort vehicles by daily rate");
            Console.WriteLine("4. Register customer");
            Console.WriteLine("5. Rent a vehicle");
            Console.WriteLine("6. Cancel rental");
            Console.WriteLine("7. Return vehicle and checkout");
            Console.WriteLine("8. Exit");
            Console.Write("Enter your choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Please enter a valid menu number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    service.DisplayAllVehicles();
                    break;

                case 2:
                    Console.Write("Enter vehicle type (Car/Bike): ");
                    string vehicleType = Console.ReadLine() ?? "";
                    service.SearchAvailableVehicles(vehicleType);
                    break;

                case 3:
                    service.SortVehiclesByRate();
                    break;

                case 4:
                    Console.Write("Enter customer name: ");
                    string name = Console.ReadLine() ?? "";

                    Console.Write("Enter email: ");
                    string email = Console.ReadLine() ?? "";

                    Console.Write("Enter 10-digit phone number: ");
                    string phone = Console.ReadLine() ?? "";

                    Console.Write("Enter driving license number: ");
                    string license = Console.ReadLine() ?? "";

                    try
                    {
                        Customer customer = new(
                            ++customerCounter,
                            name, email, phone, license);

                        customers.Add(customer);

                        Console.WriteLine(
                            $"Customer registered. ID: {customer.CustomerId}");
                    }
                    catch (ArgumentException ex)
                    {
                        customerCounter--;
                        Console.WriteLine($"Registration failed: {ex.Message}");
                    }

                    break;

                case 5:
                    if (customers.Count == 0)
                    {
                        Console.WriteLine("Please register a customer first.");
                        break;
                    }

                    Console.WriteLine("\nRegistered customers:");

                    foreach (Customer customer in customers)
                    {
                        customer.DisplayDetails();
                    }

                    Console.Write("Enter customer ID: ");

                    if (!int.TryParse(Console.ReadLine(), out int customerId))
                    {
                        Console.WriteLine("Invalid customer ID.");
                        break;
                    }

                    Customer? selectedCustomer =
                        customers.Find(c => c.CustomerId == customerId);

                    if (selectedCustomer is null)
                    {
                        Console.WriteLine("Customer not found.");
                        break;
                    }

                    service.DisplayAllVehicles();

                    Console.Write("Enter vehicle ID: ");

                    if (!VehicleRentalService.TryParseVehicleId(
                        Console.ReadLine() ?? "", out int vehicleId))
                    {
                        Console.WriteLine("Invalid vehicle ID.");
                        break;
                    }

                    Console.Write("Enter rental days: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int rentalDays))
                    {
                        Console.WriteLine("Invalid rental days.");
                        break;
                    }

                    service.CreateRental(
                        vehicleId, selectedCustomer, rentalDays,
                        ref rentalCounter);

                    break;

                case 6:
                    service.DisplayActiveRentals();

                    Console.Write("Enter rental ID to cancel: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int cancelRentalId))
                    {
                        Console.WriteLine("Invalid rental ID.");
                        break;
                    }

                    service.CancelRental(cancelRentalId);
                    break;

                case 7:
                    Console.Write("Enter rental ID to return: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int returnRentalId))
                    {
                        Console.WriteLine("Invalid rental ID.");
                        break;
                    }

                    Rental? rental = service.FindRental(returnRentalId);

                    if (rental is null || !rental.IsActive)
                    {
                        Console.WriteLine("Active rental not found.");
                        break;
                    }

                    Console.WriteLine("1. Cash payment");
                    Console.WriteLine("2. Card payment");
                    Console.Write("Choose payment method: ");

                    if (!int.TryParse(
                        Console.ReadLine(), out int paymentChoice))
                    {
                        Console.WriteLine("Invalid payment choice.");
                        break;
                    }

                    IPaymentProcessor? processor = paymentChoice switch
                    {
                        1 => new CashPaymentProcessor(),
                        2 => new CardPaymentProcessor(),
                        _ => null
                    };

                    if (processor is null)
                    {
                        Console.WriteLine("Choose option 1 or 2.");
                        break;
                    }

                    string paymentMethod =
                        paymentChoice == 1 ? "Cash" : "Card";

                    service.ReturnVehicle(
                        rental, processor, paymentMethod);

                    break;

                case 8:
                    Console.WriteLine("Thank you for using the system.");
                    return;

                default:
                    Console.WriteLine("Choose a number between 1 and 8.");
                    break;
            }
        }
    }
}
