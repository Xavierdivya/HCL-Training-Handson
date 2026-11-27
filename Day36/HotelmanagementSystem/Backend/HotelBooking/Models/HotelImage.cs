
namespace HotelBooking.Models;

public class HotelImage
{
    public int HotelImageId { get; set; }

    public int HotelId { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public bool IsPrimary { get; set; }

    // Navigation property
    public Hotel Hotel { get; set; } = null!;
}
