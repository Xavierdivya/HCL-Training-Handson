namespace TrainingManagementSystem.Payments;

public class ClassroomPaymentProcessor : IPaymentProcessor
{
    string IPaymentProcessor.MethodName => "Classroom counter";

    bool IPaymentProcessor.ProcessPayment(decimal amount)
    {
        Console.WriteLine(
            $"Simulating classroom-counter payment of ₹{amount:F2}...");

        // Explicit interface implementation.
        return amount > 0 && amount % 2 != 0;
    }
}