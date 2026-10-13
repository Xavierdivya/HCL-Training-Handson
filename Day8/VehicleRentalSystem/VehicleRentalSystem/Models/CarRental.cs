
namespace VehicleRentalSystem.Models;

public class CarRental : Rental
{
    public CarRental(int rentalId, Customer customer,
                     Vehicle vehicle, RentalPeriod period)
        : base(rentalId, customer, vehicle, period)
    {
    }

    public override decimal CalculateRentalFee()
    {
        return 500m;
    }

    public override decimal CalculateDiscount(decimal subtotal)
    {
        return subtotal >= 5000m ? subtotal * 0.05m : 0m;
    }
}
