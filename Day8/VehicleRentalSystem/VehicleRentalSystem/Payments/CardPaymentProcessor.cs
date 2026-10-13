
namespace VehicleRentalSystem.Payments;

public class CardPaymentProcessor : IPaymentProcessor
{
    bool IPaymentProcessor.ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Simulated card payment: ₹{amount:F2}");
        Console.WriteLine("Card payment approved (simulation).");
        return true;
    }
}
