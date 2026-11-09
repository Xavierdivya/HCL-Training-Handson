
using System.ComponentModel.DataAnnotations;
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.DTOs.JobApplications
{
    public class JobApplicationStatusUpdateDto
    {
        [Required]
        public ApplicationStatus Status { get; set; }

        [StringLength(1000)]
        public string? Remarks { get; set; }
    }
}
