
namespace VehicleRentalSystem.Models;

public class Customer
{
    public int CustomerId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public string LicenseNumber { get; private set; }

    public Customer(int customerId, string name, string email,
                    string phone, string licenseNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name is required.");

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Enter a valid email.");

        if (string.IsNullOrWhiteSpace(phone) ||
            phone.Length != 10 || !phone.All(char.IsDigit))
            throw new ArgumentException("Enter a 10-digit phone number.");

        if (string.IsNullOrWhiteSpace(licenseNumber))
            throw new ArgumentException("License number is required.");

        this.CustomerId = customerId;
        this.Name = name.Trim();
        this.Email = email.Trim();
        this.Phone = phone;
        this.LicenseNumber = licenseNumber.Trim();
    }

    public void DisplayDetails()
    {
        Console.WriteLine(
            $"Customer ID: {CustomerId} | Name: {Name} | " +
            $"Phone: {Phone} | License: {LicenseNumber}");
    }
}
