
namespace VehicleRentalSystem.Models;

public readonly struct VehicleLocation
{
    public string BranchName { get; }
    public string ParkingSlot { get; }

    public VehicleLocation(string branchName, string parkingSlot)
    {
        BranchName = branchName;
        ParkingSlot = parkingSlot;
    }

    public override string ToString()
    {
        return $"{BranchName}, Slot {ParkingSlot}";
    }
}
