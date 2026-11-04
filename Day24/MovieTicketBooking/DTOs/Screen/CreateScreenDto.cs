using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Screens;

public class CreateScreenDto
{
    [Range(1, int.MaxValue)]
    public int TheatreId { get; set; }

    [Required]
    [StringLength(100)]
    public string ScreenName { get; set; } = string.Empty;
}