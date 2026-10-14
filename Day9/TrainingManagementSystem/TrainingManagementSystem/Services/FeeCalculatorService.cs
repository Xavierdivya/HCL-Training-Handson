
using System;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Services;

public static class FeeCalculatorService
{
    private const decimal EarlyBirdRate = 0.10m;
    private const decimal GroupRate = 0.05m;
    private const decimal OnlineSurcharge = 200m;
    private const decimal ClassroomSurcharge = 800m;

    public static (decimal BaseFee, decimal TotalDiscount, decimal FinalFee)
        CalculateFee(
            decimal baseFee,
            int daysUntilStart,
            int groupSize,
            bool isOnline)
    {
        if (baseFee < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(baseFee), "Base fee cannot be negative.");
        }

        if (daysUntilStart < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(daysUntilStart), "Days until start cannot be negative.");
        }

        if (groupSize < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(groupSize), "Group size must be at least one.");
        }

        decimal earlyBirdDiscount =
            daysUntilStart >= 14 ? baseFee * EarlyBirdRate : 0m;

        decimal groupDiscount =
            groupSize >= 3 ? baseFee * GroupRate : 0m;

        decimal totalDiscount = earlyBirdDiscount + groupDiscount;
        decimal surcharge = isOnline ? OnlineSurcharge : ClassroomSurcharge;
        decimal finalFee = baseFee - totalDiscount + surcharge;

        return (baseFee, totalDiscount, finalFee);
    }

    public static FeeSummary CreateSummary(
        decimal baseFee,
        decimal totalDiscount,
        decimal finalFee)
    {
        decimal surcharge = finalFee - (baseFee - totalDiscount);

        return new FeeSummary(
            baseFee,
            totalDiscount,
            surcharge,
            finalFee);
    }

    public static void DisplayFeeBreakdown(in FeeSummary summary)
    {
        Console.WriteLine("\n===== FEE BREAKDOWN =====");
        Console.WriteLine($"Base Fee       : ₹{summary.BaseFee:F2}");
        Console.WriteLine($"Total Discount : ₹{summary.TotalDiscount:F2}");
        Console.WriteLine($"Session Charge : ₹{summary.Surcharge:F2}");
        Console.WriteLine($"Final Fee      : ₹{summary.FinalFee:F2}");
    }
}
