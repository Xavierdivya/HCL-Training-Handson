namespace TrainingManagementSystem.Models;

public readonly struct FeeSummary
{
    public decimal BaseFee { get; }
    public decimal TotalDiscount { get; }
    public decimal Surcharge { get; }
    public decimal FinalFee { get; }

    public FeeSummary(
        decimal baseFee,
        decimal totalDiscount,
        decimal surcharge,
        decimal finalFee)
    {
        BaseFee = baseFee;
        TotalDiscount = totalDiscount;
        Surcharge = surcharge;
        FinalFee = finalFee;
    }
}