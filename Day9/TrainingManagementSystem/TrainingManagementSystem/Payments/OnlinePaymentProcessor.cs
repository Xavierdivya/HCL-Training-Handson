namespace TrainingManagementSystem.Payments;

public class OnlinePaymentProcessor : IPaymentProcessor
{
    public string MethodName => "Online";

    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine(
            $"Simulating online payment of ₹{amount:F2}...");

        // Simulated payment only; no real money is processed.
        return amount > 0 && amount % 2 == 0;
    }
}