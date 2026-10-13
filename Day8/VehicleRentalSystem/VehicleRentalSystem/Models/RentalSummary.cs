
namespace VehicleRentalSystem.Models;

public record RentalSummary(
    int RentalId,
    string CustomerName,
    int VehicleId,
    string VehicleName,
    int RentalDays,
    decimal Subtotal,
    decimal Discount,
    decimal Fees,
    decimal Total,
    string PaymentMethod);
