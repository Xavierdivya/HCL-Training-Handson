
namespace VehicleRentalSystem.Models;

public class BikeRental : Rental
{
    public BikeRental(int rentalId, Customer customer,
                      Vehicle vehicle, RentalPeriod period)
        : base(rentalId, customer, vehicle, period)
    {
    }

    public override decimal CalculateRentalFee()
    {
        return 100m;
    }

    public override decimal CalculateDiscount(decimal subtotal)
    {
        return subtotal >= 2000m ? subtotal * 0.03m : 0m;
    }
}
