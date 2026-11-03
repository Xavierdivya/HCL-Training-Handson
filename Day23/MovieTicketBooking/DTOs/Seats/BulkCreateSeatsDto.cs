using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Seats;

public class BulkCreateSeatsDto
{
    [Required]
    [MinLength(1)]
    public List<CreateSeatDto> Seats { get; set; } = new();
}