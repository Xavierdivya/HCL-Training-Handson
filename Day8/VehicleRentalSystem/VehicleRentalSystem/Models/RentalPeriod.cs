
namespace VehicleRentalSystem.Models;

public readonly struct RentalPeriod
{
    public int RentalDays { get; }

    public RentalPeriod(int rentalDays)
    {
        if (rentalDays <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(rentalDays), "Rental days must be greater than zero.");

        RentalDays = rentalDays;
    }
}
