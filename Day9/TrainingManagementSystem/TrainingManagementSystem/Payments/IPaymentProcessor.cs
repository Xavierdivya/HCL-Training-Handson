namespace TrainingManagementSystem.Payments;

public interface IPaymentProcessor
{
    string MethodName { get; }

    bool ProcessPayment(decimal amount);
}