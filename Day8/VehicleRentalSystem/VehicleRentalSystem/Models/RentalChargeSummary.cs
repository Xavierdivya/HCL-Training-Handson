
namespace VehicleRentalSystem.Models;

public readonly struct RentalChargeSummary
{
    public decimal Subtotal { get; }
    public decimal Discount { get; }
    public decimal Fees { get; }
    public decimal FinalAmount { get; }

    public RentalChargeSummary(decimal subtotal, decimal discount,
                               decimal fees, decimal finalAmount)
    {
        Subtotal = subtotal;
        Discount = discount;
        Fees = fees;
        FinalAmount = finalAmount;
    }
}
