
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IPlacementDriveRepository
    {
        Task<IEnumerable<PlacementDrive>> GetAllAsync();

        Task<IEnumerable<PlacementDrive>> GetByCollegeIdAsync(
            int collegeId);

        Task<IEnumerable<PlacementDrive>> GetByPlacementOfficerIdAsync(
            int placementOfficerId);

        Task<PlacementDrive?> GetByIdAsync(int id);

        Task AddAsync(PlacementDrive placementDrive);

        Task SaveChangesAsync();
    }
}
