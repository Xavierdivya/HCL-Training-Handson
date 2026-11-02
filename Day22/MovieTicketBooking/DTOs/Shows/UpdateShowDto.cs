using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Shows;

public class UpdateShowDto
{
    [Range(1, int.MaxValue)]
    public int MovieId { get; set; }

    [Range(1, int.MaxValue)]
    public int ScreenId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    [Range(typeof(decimal), "0.01", "100000")]
    public decimal TicketPrice { get; set; }
}