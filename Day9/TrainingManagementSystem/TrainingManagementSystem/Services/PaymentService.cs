using TrainingManagementSystem.Models;
using TrainingManagementSystem.Payments;
using TrainingManagementSystem.Utilities;

namespace TrainingManagementSystem.Services;

public class PaymentService
{
    private readonly List<Payment> _payments = new();
    private int _nextPaymentId = 1;

    public IReadOnlyList<Payment> Payments => _payments;

    public void ProcessPayment(EnrollmentService enrollmentService)
    {
        int enrollmentId = InputValidator.ReadPositiveInteger(
            "Enrollment ID: ");

        Enrollment? enrollment =
            enrollmentService.FindEnrollment(enrollmentId);

        if (enrollment == null)
        {
            Console.WriteLine("Enrollment not found.");
            return;
        }

        if (enrollment.Status == EnrollmentStatus.Cancelled)
        {
            Console.WriteLine(
                "Cannot pay for a cancelled enrollment.");
            return;
        }

        if (enrollment.PaymentStatus == PaymentStatus.Successful)
        {
            Console.WriteLine("This enrollment is already paid.");
            return;
        }

        Console.WriteLine("1. Online payment simulation");
        Console.WriteLine("2. Classroom counter simulation");

        int choice = InputValidator.ReadPositiveInteger(
            "Choose payment method: ");

        IPaymentProcessor processor;

        if (choice == 1)
        {
            processor = new OnlinePaymentProcessor();
        }
        else if (choice == 2)
        {
            processor = new ClassroomPaymentProcessor();
        }
        else
        {
            Console.WriteLine("Invalid payment method.");
            return;
        }

        bool success = processor.ProcessPayment(enrollment.FinalFee);

        PaymentStatus status = success
            ? PaymentStatus.Successful
            : PaymentStatus.Failed;

        string method = processor.MethodName;

        _payments.Add(new Payment(
            _nextPaymentId++,
            enrollmentId,
            enrollment.FinalFee,
            DateTime.Now,
            status,
            method));

        if (success)
        {
            enrollment.PaymentStatus = PaymentStatus.Successful;
        }

        Console.WriteLine(
            success
                ? "Simulated payment successful."
                : "Simulated payment failed. Attempt recorded.");
    }

    public void DisplayHistory()
    {
        Console.WriteLine("\n===== PAYMENT HISTORY =====");

        if (_payments.Count == 0)
        {
            Console.WriteLine("No payment attempts recorded.");
            return;
        }

        foreach (Payment p in _payments)
        {
            Console.WriteLine(
                $"Payment {p.PaymentId} | Enrollment {p.EnrollmentId} | " +
                $"₹{p.Amount:F2} | {p.Method} | {p.Status} | " +
                $"{p.PaymentDate:dd-MM-yyyy HH:mm}");
        }
    }
}