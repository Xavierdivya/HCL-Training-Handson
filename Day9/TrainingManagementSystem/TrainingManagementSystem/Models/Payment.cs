namespace TrainingManagementSystem.Models;

public class Payment
{
    public int PaymentId { get; set; }
    public int EnrollmentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
    public string Method { get; set; }

    public Payment(
        int paymentId,
        int enrollmentId,
        decimal amount,
        DateTime paymentDate,
        PaymentStatus status,
        string method)
    {
        PaymentId = paymentId;
        EnrollmentId = enrollmentId;
        Amount = amount;
        PaymentDate = paymentDate;
        Status = status;
        Method = method;
    }
}