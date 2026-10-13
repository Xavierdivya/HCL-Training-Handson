
namespace VehicleRentalSystem.Models;

public abstract class Rental
{
    public int RentalId { get; }
    public Customer Customer { get; }
    public Vehicle Vehicle { get; }
    public RentalPeriod Period { get; }

    public bool IsActive { get; private set; }
    public bool IsReturned { get; private set; }
    public bool IsCancelled { get; private set; }

    protected Rental(int rentalId, Customer customer,
                     Vehicle vehicle, RentalPeriod period)
    {
        RentalId = rentalId;
        Customer = customer;
        Vehicle = vehicle;
        Period = period;

        IsActive = true;
        IsReturned = false;
        IsCancelled = false;
    }

    public decimal CalculateRentalCost()
    {
        return Vehicle.DailyRentalRate * Period.RentalDays;
    }

    public abstract decimal CalculateRentalFee();

    public virtual decimal CalculateDiscount(decimal subtotal)
    {
        return 0m;
    }

    public void CancelRental()
    {
        if (!IsActive)
            throw new InvalidOperationException("Rental is not active.");

        IsActive = false;
        IsCancelled = true;
        Vehicle.IsAvailable = true;
    }

    public void CompleteReturn()
    {
        if (!IsActive)
            throw new InvalidOperationException("Rental is not active.");

        IsActive = false;
        IsReturned = true;
        Vehicle.IsAvailable = true;
    }
}
