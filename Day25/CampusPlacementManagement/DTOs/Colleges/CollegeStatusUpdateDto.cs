
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Colleges
{
    public class CollegeStatusUpdateDto
    {
        [StringLength(500)]
        public string? Remarks { get; set; }
    }
}
