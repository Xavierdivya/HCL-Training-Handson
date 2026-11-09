
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.JobApplications
{
    public class JobApplicationCreateDto
    {
        [Required]
        public int JobId { get; set; }

        [StringLength(2000)]
        public string? CoverLetter { get; set; }
    }
}
